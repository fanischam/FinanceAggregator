namespace MockBankApi.Models
{
    public record BetaTransaction(
        string transaction_id,
        string account_id,
        int amount_cents,
        string currency_code,
        string vendor_name,
        string transaction_date);
}
