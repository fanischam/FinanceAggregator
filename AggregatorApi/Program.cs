using AggregatorApi.Adapters;
using AggregatorApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Registering Http Clients, one for each bank
builder.Services.AddHttpClient<AlphaTransactionAdapter>(c =>
    c.BaseAddress = new Uri("https://localhost:7059/"));

builder.Services.AddHttpClient<BetaTransactionAdapter>(c =>
    c.BaseAddress = new Uri("https://localhost:7059/"));

builder.Services.AddHttpClient<GammaTransactionAdapter>(c =>
    c.BaseAddress = new Uri("https://localhost:7059/"));

builder.Services.AddTransient<ITransactionAdapter, AlphaTransactionAdapter>();
builder.Services.AddTransient<ITransactionAdapter, BetaTransactionAdapter>();
builder.Services.AddTransient<ITransactionAdapter, GammaTransactionAdapter>();

builder.Services.AddTransient<AggregatorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();

