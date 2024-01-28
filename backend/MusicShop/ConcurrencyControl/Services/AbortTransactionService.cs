using MusicShop.ConcurrencyControl.Command;
using MusicShop.ConcurrencyControl.Enums;
using MusicShop.ConcurrencyControl.Models;

namespace MusicShop.ConcurrencyControl.Services;

public class AbortTransactionService
{
    private readonly ConcurrencyControlService _concurrencyControlService;
    private readonly Dictionary<int, List<ICommand>> _rollbackCommandsByTransactionId;
    private readonly object _rollbacksLock = new object();

    public AbortTransactionService(ConcurrencyControlService concurrencyControlService)
    {
        _concurrencyControlService = concurrencyControlService;
        _rollbackCommandsByTransactionId = new Dictionary<int, List<ICommand>>();
    }

    public async Task AbortTransaction(Transaction transaction)
    {
        _concurrencyControlService.AbortTransaction(transaction);
        await RollbackTransaction(transaction.Id);
    }

    public void InsertRollbackCommand(int transactionId, ICommand command)
    {
        lock (_rollbacksLock)
        {
            var transactionExists = _rollbackCommandsByTransactionId.TryGetValue(transactionId, out var commands);
            if (transactionExists)
            {
                commands.Add(command);
            }
            else
            {
                _rollbackCommandsByTransactionId[transactionId] = new List<ICommand>() { command };
            }
        }
    }
    
    private async Task RollbackTransaction(int transactionId)
    {
        var requiredTableLocks = new List<Tuple<Table, OperationType>>();
        foreach (var command in _rollbackCommandsByTransactionId[transactionId])
        {
            requiredTableLocks.AddRange(command.RequiredTables);
        }
        
        var transaction = new Transaction(requiredTableLocks);
        _concurrencyControlService.AddNewTransaction(transaction);
        _concurrencyControlService.BlockTablesForTransaction(transaction);
        
        foreach (var command in _rollbackCommandsByTransactionId[transactionId])
        {
            await command.Execute();
            InsertRollbackCommand(transaction.Id, command.GetOppositeOperation());
        }

        _concurrencyControlService.CommitTransaction(transaction);
    }
}