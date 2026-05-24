using System.Text.Json;
using FraudAPI.Configuration;
using FraudAPI.Dtos;
using FraudAPI.Infrastructure.Qdrant;
using FraudAPI.Normalizers;
using FraudAPI.Services;
using Qdrant.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var normalizationPath = Path.Combine(AppContext.BaseDirectory, "Resources", "normalization.json");
var normalizationConstantsJson = File.ReadAllText(normalizationPath);
var normalizationConstants = JsonSerializer.Deserialize<NormalizationConstants>(normalizationConstantsJson);
builder.Services.AddSingleton(normalizationConstants!);

var path = Path.Combine(AppContext.BaseDirectory, "Resources", "mcc_risk.json");
builder.Services.AddSingleton(_ => new MccRiskTable(path));

var qdrantConfiguration = builder.Configuration
    .GetSection("Qdrant")
    .Get<QdrantConfiguration>()!;

builder.Services.AddSingleton(qdrantConfiguration);
builder.Services.AddSingleton(new QdrantClient(
    new Uri($"http://{qdrantConfiguration.Host}:{qdrantConfiguration.Port}")
));

builder.Services.AddSingleton<QdrantCollectionInitializer>();

builder.Services.AddSingleton<QdrantSeeder>();

builder.Services.AddSingleton<ITransactionVectorRepository, QdrantTransactionRepository>();

builder.Services.AddSingleton<IKnnFraudClassifier, KnnFraudClassifier>();

builder.Services.AddSingleton<INormalizationStep, TransactionNormalizer>();
builder.Services.AddSingleton<INormalizationStep, CustomerNormalizer>();
builder.Services.AddSingleton<INormalizationStep, MerchantNormalizer>();
builder.Services.AddSingleton<INormalizationStep, TerminalNormalizer>();
builder.Services.AddSingleton<INormalizationStep, LastTransactionNormalizer>();
builder.Services.AddSingleton<INormalizationPipeline, NormalizationPipeline>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var initializer = app.Services.GetRequiredService<QdrantCollectionInitializer>();
await initializer.InitializeAsync();

var runSeeder = Environment.GetEnvironmentVariable("RUN_SEEDER") == "true";

if (runSeeder)
{
    var seeder = app.Services.GetRequiredService<QdrantSeeder>();
    var datasetPath = Path.Combine(AppContext.BaseDirectory, "Resources", "references.json");
    await seeder.SeedAsync(datasetPath);
}

app.MapGet("/ready", () => Results.Ok())
.WithName("Ready");

app.MapPost("/fraud-score", async (FraudRequestDto request, HttpContext context) =>
{
    var pipeline = context.RequestServices.GetRequiredService<INormalizationPipeline>();
    var classifier = context.RequestServices.GetRequiredService<IKnnFraudClassifier>();
    var normalizedArray = pipeline.Normalize(request);
    var result = await classifier.ClassifyAsync(normalizedArray);
    return Results.Ok(result);
})
.WithName("Fraud-score");

app.Run();