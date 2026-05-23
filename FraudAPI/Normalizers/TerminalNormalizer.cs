using FraudAPI.Configuration;

namespace FraudAPI.Normalizers;

public class TerminalNormalizer(NormalizationConstants normalizationConstants)
    : INormalizationStep
{
    private readonly NormalizationConstants _normalizationConstants = normalizationConstants;

    public void ExecuteNormalization(NormalizationContext context, NormalizedFeatures features)
    {
        features.KmFromHome = NormalizeKmFromHome(context.Terminal.KmFromHome);
        features.IsOnline = context.Terminal.IsOnline ? 1 : 0;
        features.CardPresent = context.Terminal.CardPresent ? 1 : 0;
    }

    private double NormalizeKmFromHome(double kmFromHome)
    {
        var ratio = kmFromHome / _normalizationConstants.MaxKm;
        return _normalizationConstants.Clamp(ratio);
    }
}