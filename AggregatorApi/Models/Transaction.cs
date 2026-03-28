namespace AggregatorApi.Models
{
    public class Transaction
    {
        public string Id { get; init; } = default!;
        public string AccountId { get; init; } = default!;
        public string BankSource { get; init; } = default!;
        public decimal AmountEur { get; init; }
        public string Currency { get; init; } = default!;
        public string MerchantName { get; init; } = default!;
        public DateTimeOffset PostedAt { get; init; }
    }
}
