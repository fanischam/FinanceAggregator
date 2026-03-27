namespace MockBankApi.Models
{
    record GammaTransaction(
        int id,
        string AccountNumber,
        string Value,
        string CurrencyCode,
        string Description,
        String PostedAt);
}
