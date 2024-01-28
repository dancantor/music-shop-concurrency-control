using MusicShop.ConcurrencyControl.Enums;

namespace MusicShop.ConcurrencyControl.Services;

public class DeadlockDetectionService : IHostedService, IDisposable
{
    private readonly ILogger<DeadlockDetectionService> _logger;
    private readonly ConcurrencyControlService _concurrencyControlService;
    private readonly AbortTransactionService _abortTransactionService;
    private Timer _timer;

    public DeadlockDetectionService(ILogger<DeadlockDetectionService> logger, ConcurrencyControlService concurrencyControlService, AbortTransactionService abortTransactionService)
    {
        _logger = logger;
        _concurrencyControlService = concurrencyControlService;
        _abortTransactionService = abortTransactionService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deadlock Detection Service running.");
        _timer = new Timer(DetectCycles, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        return Task.CompletedTask;
    }

    private async void DetectCycles(object? state)
    {
        var waitForGraphs = _concurrencyControlService.GetWaitForGraph();
        var transactions = _concurrencyControlService.GetActiveTransactions();
        var waitForGraphByTransactionId = new Dictionary<int, HashSet<int>>();
        foreach (var waitForGraph in waitForGraphs)
        {
            if (waitForGraph.TransactionIdThatHasLock is null)
            {
                continue;
            }

            foreach (var waitingTransaction in waitForGraph.TransactionIdsThatWaitForLock)
            {
                waitForGraphByTransactionId.TryGetValue(waitingTransaction, out var dependingOnTransactions);
                if (dependingOnTransactions is null)
                {
                    waitForGraphByTransactionId[waitingTransaction] = new HashSet<int>()
                        { waitForGraph.TransactionIdThatHasLock.Value };
                }
                else
                {
                    dependingOnTransactions.Add(waitForGraph.TransactionIdThatHasLock.Value);
                }
            }
        }

        foreach (var transactionFirst in transactions)
        {
            foreach (var transactionSecond in transactions)
            {
                if (transactionFirst.Id == transactionSecond.Id || transactionFirst.TransactionStatus != Status.Active || transactionSecond.TransactionStatus != Status.Active)
                {
                    continue;
                }

                if (waitForGraphByTransactionId[transactionFirst.Id].Contains(transactionSecond.Id) &&
                    waitForGraphByTransactionId[transactionSecond.Id].Contains(transactionFirst.Id))
                {
                    _logger.LogInformation($"Transaction with id {transactionSecond.Id} was aborted");
                    await _abortTransactionService.AbortTransaction(transactionSecond);
                }
            }
        }
        
        _logger.LogInformation("Finished detecting for cycles");

    }
    

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deadlock Detection Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}