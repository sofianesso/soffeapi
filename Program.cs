using Microsoft.EntityFrameworkCore;
using myOrderApi.Data;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Azure Key Vault config
var keyVaultUrl = "https://munchmeister-kv.vault.azure.net/";
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Basic services + mvc
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string for Cosmos DB from AZ Key Vault
var cosmosDbConnectionString = builder.Configuration["munchmeister-connectionstring"];
var databaseName = "munchmeister-db";

// Connection string control
if (string.IsNullOrEmpty(cosmosDbConnectionString))
{
    throw new InvalidOperationException("Cosmos DB-anslutningssträngen är inte konfigurerad.");
}

// Entity Framework config for CosmosDB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseCosmos(cosmosDbConnectionString, databaseName));

var app = builder.Build();

// Konfigurerar HTTP request pipeline med utvecklingsverktyg som Swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
