using AggregatorApi.Models;

namespace AggregatorApi.Adapters
{
    public class GammaTransactionAdapter : ITransactionAdapter
    {
        private readonly HttpClient _client;
        public string BankName => "gamma";

        public GammaTransactionAdapter(HttpClient client) => _client = client;

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountId, CancellationToken ct = default)
        {
            var response = await _client.GetFromJsonAsync<List<GammaRawTransaction>>($"/gamma/accounts/{accountId}/transactions", ct) ?? [];

            return response.Select(r => new Transaction
            {
                Id = r.Id.ToString(),
                AccountId = r.AccountNumber,
                BankSource = BankName,
                AmountEur = ConvertToEur(decimal.Parse(r.Value), r.CurrencyCode),
                Currency = r.CurrencyCode,
                MerchantName = r.Description,
                PostedAt = DateTimeOffset.Parse(r.PostedAt)
            });
        }

        /// <summary>
        /// Converts the given amount from the specified currency to EUR using hardcoded exchange rates.
        /// </summary>
        /// <param name="amount">The amount to be converted</param>
        /// <param name="currencyCode">The currency code of the amount</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws if currency code is not recognized</exception>
        private static decimal ConvertToEur(decimal amount, string currencyCode)
        {
            var exchangeRates = new Dictionary<string, decimal> {
                { "USD", 0.85m },
                { "GBP", 1.15m },
                { "EUR", 1m}
            };

            if (exchangeRates.TryGetValue(currencyCode.ToUpper(), out var rate))
            {
                return Math.Round(amount * rate, 2);
            }
            throw new InvalidOperationException($"Unsupported currency: {currencyCode}");
        }

        private record GammaRawTransaction(
        int Id,
        string AccountNumber,
        string Value,
        string CurrencyCode,
        string Description,
        string PostedAt);
    }
}
