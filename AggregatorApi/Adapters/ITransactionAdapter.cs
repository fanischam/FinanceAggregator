using AggregatorApi.Models;

namespace AggregatorApi.Adapters
{
    public interface ITransactionAdapter
    {
        string BankName { get; }
        Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountId, CancellationToken ct = default);
    }
}
