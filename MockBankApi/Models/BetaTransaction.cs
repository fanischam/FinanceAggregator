namespace MockBankApi.Models
{
    public class BetaTransaction
    {
        public string transaction_id { get; set; }
        public string account_id { get; set; }
        public int amount_cents { get; set; }
        public string currency_code { get; set; }
        public string vendor_name { get; set; }
        public string transaction_date { get; set; }
    }
}
