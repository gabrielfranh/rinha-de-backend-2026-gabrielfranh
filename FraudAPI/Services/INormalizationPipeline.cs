using FraudAPI.Dtos;

namespace FraudAPI.Services;

public interface INormalizationPipeline
{
    double[] Normalize(FraudRequestDto fraudRequestDto);
}