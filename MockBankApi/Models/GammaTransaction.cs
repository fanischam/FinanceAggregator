namespace MockBankApi.Models
{
    record GammaTransaction(
        int Id,
        string AccountNumber,
        string Value,
        string CurrencyCode,
        string Description,
        String PostedAt);
}
