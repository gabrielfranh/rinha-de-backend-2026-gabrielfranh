using FraudAPI.Dtos;
using FraudAPI.Infrastructure.Qdrant;

namespace FraudAPI.Services;

public interface IKnnFraudClassifier
{
    Task<BaseResponseDto> ClassifyAsync(double[] normalizedArray);
}

public class KnnFraudClassifier(ITransactionVectorRepository repository) : IKnnFraudClassifier
{
    private const int K = 5;
    private const double FraudThreshold = 0.5;

    public async Task<BaseResponseDto> ClassifyAsync(double[] normalizedArray)
    {
        var neighbors = await repository.FindKNearestAsync(normalizedArray, K);

        if (neighbors.Count == 0)
            return new BaseResponseDto(Approved: true, FraudScore: 0.0);

        var fraudCount = neighbors.Count(n =>
            n.Payload.TryGetValue("is_fraud", out var value) && value.BoolValue);

        var fraudRatio = (double)fraudCount / neighbors.Count;

        return new BaseResponseDto(
            Approved: fraudRatio < FraudThreshold,
            FraudScore: fraudRatio);
    }
}