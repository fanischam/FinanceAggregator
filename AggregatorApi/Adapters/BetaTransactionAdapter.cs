using AggregatorApi.Models;
using System.Text.Json.Serialization;

namespace AggregatorApi.Adapters
{
    public class BetaTransactionAdapter : ITransactionAdapter
    {
        private readonly HttpClient _client;
        public string BankName => "beta";

        public BetaTransactionAdapter(HttpClient client) => _client = client;

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string accountId, CancellationToken ct = default)
        {
            var response = await _client.GetFromJsonAsync<List<BetaRawTransaction>>($"beta/accounts/{accountId}/transactions", ct) ?? [];

            return response.Select(r => new Transaction
            {
                Id = r.TransactionId,
                AccountId = r.AccountRef,
                AmountEur = ConvertToEur(r.AmountCents, r.CurrencyCode),
                BankSource = BankName,
                Currency = r.CurrencyCode,
                MerchantName = r.VendorName,
                PostedAt = DateTimeOffset.Parse(r.TransactionDate)
            });
        }

        /// <summary>
        /// Converts the given amount from the specified currency to EUR using hardcoded exchange rates.
        /// </summary>
        /// <param name="amountCents">The amount in cents</param>
        /// <param name="currencyCode">The currency code</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Throws if incorrect currency code</exception>
        private static decimal ConvertToEur(int amountCents, string currencyCode)
        {
            var exchangeRates = new Dictionary<string, decimal>
            {
                { "USD", 0.85m },
                { "GBP", 1.15m },
                { "EUR", 1m }
            };

            if (exchangeRates.TryGetValue(currencyCode.ToUpper(), out var rate))
            {
                var amount = amountCents / 100m;
                return Math.Round(amount * rate, 2);
            }
            throw new InvalidOperationException($"Unsupported currency: {currencyCode}");
        }

        private record BetaRawTransaction(
        [property: JsonPropertyName("transaction_id")] string TransactionId,
        [property: JsonPropertyName("account_ref")] string AccountRef,
        [property: JsonPropertyName("amount_cents")] int AmountCents,
        [property: JsonPropertyName("currency_code")] string CurrencyCode,
        [property: JsonPropertyName("vendor_name")] string VendorName,
        [property: JsonPropertyName("transaction_date")] string TransactionDate);
    }
}
