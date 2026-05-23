using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace FraudAPI.Infrastructure.Qdrant;

public class QdrantCollectionInitializer(QdrantClient client, QdrantConfiguration configuration)
{
    public async Task InitializeAsync()
    {
        var exists = await client.CollectionExistsAsync(configuration.CollectionName);
        if (exists) return;

        await client.CreateCollectionAsync(configuration.CollectionName,
            new VectorParams
            {
                Size = 14,
                Distance = Distance.Cosine
            });
    }
}