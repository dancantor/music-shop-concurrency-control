using MusicShop.ConcurrencyControl.Enums;

namespace MusicShop.ConcurrencyControl.Models;

public class Lock
{
    private static int _lockCount = 0;
    private static readonly object _lockObj = _lockCount;

    public Lock(OperationType operationType, Table lockedTable, int transactionId)
    {
        lock (_lockObj)
        {
            Id = _lockCount;
            _lockCount++;
        }
        OperationType = operationType;
        LockedTable = lockedTable;
        TransactionId = transactionId;
    }

    public int Id { get; set; }
    
    public OperationType OperationType { get; set; }
    
    public Table LockedTable { get; set; }
    
    public int TransactionId { get; set; }
}