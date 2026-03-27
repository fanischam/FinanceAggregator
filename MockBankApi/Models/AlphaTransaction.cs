namespace MockBankApi.Models
{
    record AlphaTransaction(
        string TransactionId,
        string AccountId,
        decimal Amount,
        string Currency,
        string MerchantName,
        string Date);
}
