namespace FraudAPI.Infrastructure.Qdrant;

public class QdrantConfiguration
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; init; }
    public string CollectionName { get; init; } = string.Empty;
}