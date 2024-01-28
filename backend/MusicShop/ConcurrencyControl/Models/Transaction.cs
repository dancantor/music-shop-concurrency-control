using MusicShop.ConcurrencyControl.Enums;

namespace MusicShop.ConcurrencyControl.Models;

public class Transaction
{
    private static int _transactionCount = 0;
    private static readonly object _lockObj = _transactionCount;

    public Transaction(List<Tuple<Table, OperationType>> requiredTables)
    {
        lock (_lockObj)
        {
            Id = _transactionCount;
            _transactionCount++;
        }
        TransactionStatus = Status.Active;
        RequiredTables = requiredTables;
    }

    public int Id { get; set; }

    public Status TransactionStatus { get; set; }
    
    public List<Tuple<Table, OperationType>> RequiredTables { get; set; }
}