using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace FraudAPI.Infrastructure.Qdrant;

public interface ITransactionVectorRepository
{
    Task<IReadOnlyList<ScoredPoint>> FindKNearestAsync(double[] features, int k);
}

public class QdrantTransactionRepository(QdrantClient client, QdrantConfiguration configuration)
    : ITransactionVectorRepository
{
    public async Task<IReadOnlyList<ScoredPoint>> FindKNearestAsync(double[] features, int k)
    {
        return await client.SearchAsync(
            configuration.CollectionName,
            features.Select(f => (float)f).ToArray(),
            limit: (ulong)k,
            payloadSelector: true);
    }
}