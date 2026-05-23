namespace FraudAPI.Normalizers;

public interface INormalizationStep
{
    void ExecuteNormalization(NormalizationContext context, NormalizedFeatures features);
}