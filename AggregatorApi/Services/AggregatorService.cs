using AggregatorApi.Adapters;
using AggregatorApi.Models;

namespace AggregatorApi.Services
{
    public class AggregatorService(IEnumerable<ITransactionAdapter> adapters, ILogger<AggregatorService> logger)
    {
        private readonly IEnumerable<ITransactionAdapter> _adapters = adapters;
        private readonly ILogger<AggregatorService> _logger = logger;

        /// <summary>
        /// Fetches transactions for the specified account ID from all registered adapters, handling any adapter failures gracefully.
        /// </summary>
        /// <param name="accountId">The account to fetch transactions</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns>The list of transactions</returns>
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(string accountId, CancellationToken ct = default)
        {
            var tasks = _adapters.Select(async adapter =>
            {
                try
                {
                    return await adapter.GetTransactionsAsync(accountId, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Adapter {adapter.BankName} failed for account {accountId}: {ex.Message}");

                    return Enumerable.Empty<Transaction>();
                }
            });

            var results = await Task.WhenAll(tasks);

            return results.SelectMany(r => r).OrderByDescending(t => t.PostedAt);
        }

    }
}
