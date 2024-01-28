using MusicShop.ConcurrencyControl.Enums;
using MusicShop.ConcurrencyControl.Models;

namespace MusicShop.ConcurrencyControl.Services;

public class ConcurrencyControlService
{
    private readonly List<Transaction> _transactions;
    private readonly List<Lock> _locks;
    private readonly List<WaitForGraph> _waitForGraphs;
    private readonly object _transactionsLock = new object();
    private readonly object _locksLock = new object();
    private readonly object _waitForGraphsLock = new object();

    public ConcurrencyControlService()
    {
        _transactions = new List<Transaction>();
        _locks = new List<Lock>();
        _waitForGraphs = new List<WaitForGraph>();
        foreach (var operationType in Enum.GetValues(typeof(OperationType)).Cast<OperationType>())
        {
            foreach (var table in Enum.GetValues(typeof(Table)).Cast<Table>())
            {
                _waitForGraphs.Add(new WaitForGraph(operationType, table));
            }
        }
    }

    public List<WaitForGraph> GetWaitForGraph()
    {
        lock (_waitForGraphsLock)
        {
            return _waitForGraphs;
        }
    }

    public List<Transaction> GetActiveTransactions()
    {
        lock (_transactionsLock)
        {
            return _transactions.Where(transaction => transaction.TransactionStatus == Status.Active).ToList();
        } 
    }
    
    public void AddNewTransaction(Transaction transaction)
    {
        lock (_transactionsLock)
        {
            _transactions.Add(transaction);
        }
    }

    public void BlockTablesForTransaction(Transaction transaction)
    {
        lock (_locksLock)
        {
            foreach (var (table, operationType) in transaction.RequiredTables)
            {
                var hasAquiredLock = false;
                while (!hasAquiredLock)
                {
                    var lockOperation = _locks.Find(lockOp =>
                    {
                        if (operationType == OperationType.Read)
                        {
                            return lockOp.LockedTable == table && lockOp.OperationType == OperationType.Write && lockOp.TransactionId != transaction.Id;
                        }
                        return lockOp.LockedTable == table && lockOp.TransactionId != transaction.Id;
                    });
                    if (lockOperation is null)
                    {
                        _locks.Add(new Lock(operationType, table, transaction.Id));
                        lock (_waitForGraphsLock)
                        {
                            var correspondingWaitForModel = _waitForGraphs.Find(waitForGraph =>
                                waitForGraph.LockTable == table && waitForGraph.LockType == operationType);
                            correspondingWaitForModel?.TransactionIdsThatWaitForLock.RemoveAll(transactionId =>
                                transactionId == transaction.Id);
                            correspondingWaitForModel.TransactionIdThatHasLock = transaction.Id;
                        }

                        hasAquiredLock = true;
                    }
                    else
                    {
                        lock (_waitForGraphsLock)
                        {
                            var correspondingWaitForModel = _waitForGraphs.Find(waitForGraph =>
                                waitForGraph.LockTable == table && waitForGraph.LockType == operationType);
                            correspondingWaitForModel?.TransactionIdsThatWaitForLock.Add(transaction.Id);
                        }
                        
                        Monitor.Wait(_locksLock);
                    }
                }
            }
        }
    }

    public void CommitTransaction(Transaction transaction)
    {
        lock (_transactionsLock)
        {
            var committedTransaction = _transactions.Find(existingTransaction => existingTransaction.Id == transaction.Id);
            committedTransaction.TransactionStatus = Status.Commit;
        }
        lock (_locksLock)
        {
            foreach (var (table, operationType) in transaction.RequiredTables)
            {
                _locks.RemoveAll(lockOp => lockOp.LockedTable == table && lockOp.OperationType == operationType);
                lock (_waitForGraphs)
                {
                    var correspondingWaitForObj = _waitForGraphs.Find(waitForGraph =>
                        waitForGraph.LockTable == table && waitForGraph.LockType == operationType);
                    correspondingWaitForObj.TransactionIdThatHasLock = null;
                }
            }
            Monitor.PulseAll(_locksLock);
        }
    }

    public void AbortTransaction(Transaction transaction)
    {
        lock (_transactionsLock)
        {
            var transactionToAbort = _transactions.Find(currentTransaction => currentTransaction.Id == transaction.Id);
            transactionToAbort.TransactionStatus = Status.Abort;
        }

        lock (_locksLock)
        {
            _locks.RemoveAll(currentLock => currentLock.TransactionId == transaction.Id);
            lock (_waitForGraphsLock)
            {
                foreach (var waitForGraph in _waitForGraphs)
                {
                    if (waitForGraph.TransactionIdThatHasLock == transaction.Id)
                    {
                        waitForGraph.TransactionIdThatHasLock = null;
                    }

                    waitForGraph.TransactionIdsThatWaitForLock.RemoveAll(transactionId => transactionId == transaction.Id);
                }
            }
            Monitor.PulseAll(_locksLock);
        }
    }
}