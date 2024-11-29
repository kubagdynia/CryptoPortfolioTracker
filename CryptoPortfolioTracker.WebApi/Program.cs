using CryptoPortfolioTracker.Core.Extensions;
using CryptoPortfolioTracker.WebApi.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

ConfigureSerilog();

builder.Services.RegisterCryptoPortfolioTracker(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName?.Replace('+', '.')));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
return;

void ConfigureSerilog()
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
        
    // configure logging
    builder.Services.AddLogging(b =>
    { 
        b.AddSerilog();
    });
}