using FraudAPI.Dtos;
using FraudAPI.Normalizers;
using FraudAPI.Utils;

namespace FraudAPI.Services;

public class NormalizationPipeline(
    IEnumerable<INormalizationStep> normalizationSteps)
    : INormalizationPipeline
{
    public double[] Normalize(FraudRequestDto fraudRequestDto)
    {
        var fraudRequest = FraudHelper.ToModel(fraudRequestDto);
        var context = new NormalizationContext(
            fraudRequest.Transaction,
            fraudRequest.Customer,
            fraudRequest.Merchant,
            fraudRequest.Terminal,
            fraudRequest.LastTransaction
        );
        
        var features = new NormalizedFeatures();
        foreach (var step in normalizationSteps)
            step.ExecuteNormalization(context, features);

        return features.ToOrderedArray();
    }
}