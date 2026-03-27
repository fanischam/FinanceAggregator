using Bogus;
using MockBankApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

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

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
