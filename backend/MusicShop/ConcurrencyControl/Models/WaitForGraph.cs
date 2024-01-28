
using MusicShop.ConcurrencyControl.Enums;

namespace MusicShop.ConcurrencyControl.Models;

public class WaitForGraph
{
    public WaitForGraph(OperationType lockType, Table lockTable)
    {
        LockType = lockType;
        LockTable = lockTable;
        TransactionIdThatHasLock = null;
        TransactionIdsThatWaitForLock = new List<int>();
    }

    public OperationType LockType { get; set; }
    
    public Table LockTable { get; set; }
    
    public int? TransactionIdThatHasLock { get; set; }
    
    public List<int> TransactionIdsThatWaitForLock { get; set; }

    public void InsertTransactionThatWaitsForLock(int transactionId)
    {
        TransactionIdsThatWaitForLock.Add(transactionId);
    }
    
}