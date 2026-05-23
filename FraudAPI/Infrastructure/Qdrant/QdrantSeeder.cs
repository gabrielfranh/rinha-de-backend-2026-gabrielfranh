using System.Text.Json;
using System.Text.Json.Serialization;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace FraudAPI.Infrastructure.Qdrant;

public class QdrantSeeder(QdrantClient client, QdrantConfiguration configuration)
{
    public async Task SeedAsync(string datasetPath)
    {
        var collectionInfo = await client.GetCollectionInfoAsync(configuration.CollectionName);
        if (collectionInfo.PointsCount > 0)
            return;

        await using var fileStream = File.OpenRead(datasetPath);
        var entries = await JsonSerializer.DeserializeAsync<List<DatasetEntry>>(fileStream);

        if (entries is null || entries.Count == 0)
            return;

        var points = entries.Select((entry, index) => new PointStruct
        {
            Id = new PointId { Num = (ulong)index },
            Vectors = entry.Vector.Select(v => (float)v).ToArray(),
            Payload = { ["is_fraud"] = entry.Label == "fraud" }
        }).ToList();

        const int batchSize = 1000;
        foreach (var batch in points.Chunk(batchSize))
            await client.UpsertAsync(configuration.CollectionName, batch);
    }
}

public record DatasetEntry(
    [property: JsonPropertyName("vector")] List<double> Vector,
    [property: JsonPropertyName("label")] string Label
);