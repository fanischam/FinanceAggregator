using AggregatorApi.Models;

namespace AggregatorApi.Adapters
{
    public class AlphaTransactionAdapter : ITransactionAdapter
    {
        private readonly HttpClient _client;
        public string BankName => "alpha";

        public AlphaTransactionAdapter(HttpClient client) => _client = client;

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync( string accountId, CancellationToken ct = default)
        {
            var response = await _client.GetFromJsonAsync<List<AlphaRawTransaction>>($"alpha/accounts/{accountId}/transactions", ct) ?? [];

            return response.Select(r => new Transaction
            {
                Id = r.TransactionId,
                AccountId = r.AccountId,
                BankSource = BankName,
                AmountEur = ConvertToEur(r.Amount, r.Currency),
                Currency = r.Currency,
                MerchantName = r.MerchantName,
                PostedAt = DateTimeOffset.Parse(r.Date)
            });
        }

        /// <summary>
        /// Converts the given amount from the specified currency to EUR using hardcoded exchange rates.
        /// </summary>
        /// <param name="amount">The transaction amouunt</param>
        /// <param name="currency">The currency to convert the amount to</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws if currency code was incorrect</exception>
        private decimal ConvertToEur(decimal amount, string currency)
        {
            var exchangeRates = new Dictionary<string, decimal>
            {
                { "USD", 0.85m },
                { "GBP", 1.15m },
                { "EUR", 1m }
            };

            if (exchangeRates.TryGetValue(currency.ToUpper(), out var rate))
            {
                return Math.Round(amount * rate, 2);
            }
            throw new InvalidOperationException($"Unsupported currency: {currency}");
        }

        private record AlphaRawTransaction(
        string TransactionId,
        string AccountId,
        decimal Amount,
        string Currency,
        string MerchantName,
        string Date);
    }
}
