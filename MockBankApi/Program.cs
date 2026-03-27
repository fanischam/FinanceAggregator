using Bogus;
using MockBankApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("alpha/accounts/{accountId}/transactions", (string accountId) =>
{
    var faker = new Faker<AlphaTransaction>()
        .RuleFor(t => t.TransactionId, f => f.Random.Guid().ToString())
        .RuleFor(t => t.AccountId, f => accountId)
        .RuleFor(t => t.Amount, f => Math.Round(f.Finance.Amount(-200, 1500), 2))
        .RuleFor(t => t.Currency, f => "USD")
        .RuleFor(t => t.MerchantName, f => f.Company.CompanyName())
        .RuleFor(t => t.Date, f => f.Date.Past(1).ToString("yyyy-MM-dd"));

    return Results.Ok(faker.Generate(20));
});

app.MapGet("beta/accounts/{accountRef}/transactions", (string accountRef) =>
{
    var faker = new Faker<BetaTransaction>()
        .RuleFor(t => t.transaction_id, f => f.Random.Guid().ToString())
        .RuleFor(t => t.account_id, f => accountRef)
        .RuleFor(t => t.amount_cents, f => f.Random.Int(-20000, 150000))
        .RuleFor(t => t.currency_code, f => "USD")
        .RuleFor(t => t.vendor_name, f => f.Company.CompanyName())
        .RuleFor(t => t.transaction_date, f => f.Date.Past(1).ToString("yyyy-MM-dd"));

    return Results.Ok(faker.Generate(20));
});

app.MapGet("/gamma/accounts/{accountNumber}/transactions", (string accountNumber) =>
{
    var faker = new Faker<GammaTransaction>()
        .RuleFor(t => t.Id, f => f.IndexFaker + 1)
        .RuleFor(t => t.AccountNumber, f => accountNumber)
        .RuleFor(t => t.Value, f => f.Finance.Amount(-150, 2000).ToString("F2"))
        .RuleFor(t => t.CurrencyCode, f => "GBP")
        .RuleFor(t => t.Description, f => f.Commerce.ProductName())
        .RuleFor(t => t.PostedAt, f => f.Date.Past(1).ToString("o")); // ISO 8601

    return Results.Ok(faker.Generate(20));
});

app.Run();