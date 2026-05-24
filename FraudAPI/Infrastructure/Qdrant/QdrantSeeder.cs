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

        var sampled = await SampleDatasetAsync(datasetPath, maxSamples: 200_000);

        if (sampled.Count == 0)
            return;

        const int batchSize = 1000;
        foreach (var batch in sampled.Chunk(batchSize))
        {
            var points = batch.Select((entry, index) => new PointStruct
            {
                Id = new PointId { Num = (ulong)index },
                Vectors = entry.Vector.Select(v => (float)v).ToArray(),
                Payload = { ["is_fraud"] = entry.Label == "fraud" }
            }).ToList();

            await client.UpsertAsync(configuration.CollectionName, points);
        }
    }

    private static async Task<List<DatasetEntry>> SampleDatasetAsync(string path, int maxSamples)
    {
        var reservoir = new List<DatasetEntry>(maxSamples);
        var random = new Random(42);
        var index = 0;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        await using var stream = File.OpenRead(path);
        await foreach (var entry in JsonSerializer.DeserializeAsyncEnumerable<DatasetEntry>(stream, options))
        {
            if (entry is null) continue;

            if (index < maxSamples)
            {
                reservoir.Add(entry);
            }
            else
            {
                var j = random.Next(index + 1);
                if (j < maxSamples)
                    reservoir[j] = entry;
            }

            index++;
        }

        return reservoir;
    }
}

public record DatasetEntry(
    [property: JsonPropertyName("vector")] List<double> Vector,
    [property: JsonPropertyName("label")] string Label
);