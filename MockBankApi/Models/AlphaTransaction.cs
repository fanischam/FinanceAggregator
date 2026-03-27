namespace MockBankApi.Models
{
    public class AlphaTransaction
    {
        public string TransactionId { get; init; }
        public string AccountId { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; }
        public string MerchantName { get; init; }
        public string Date { get; init; }
    }
}
