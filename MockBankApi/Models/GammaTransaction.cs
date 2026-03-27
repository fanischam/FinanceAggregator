namespace MockBankApi.Models
{
    public class GammaTransaction
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string Value { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string PostedAt { get; set; }
    }
}
