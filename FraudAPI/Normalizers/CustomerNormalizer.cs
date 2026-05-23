using FraudAPI.Configuration;

namespace FraudAPI.Normalizers;

public class CustomerNormalizer (NormalizationConstants normalizationConstants)
    : INormalizationStep
{
    private readonly NormalizationConstants _normalizationConstants = normalizationConstants;
    
    public void ExecuteNormalization(NormalizationContext context, NormalizedFeatures features)
    {
        features.TxCount24h = NormalizeTxCount24H(context.Customer.TxCount24H);
    }

    private double NormalizeTxCount24H(double txCount24H)
    {
        var ratio = txCount24H / _normalizationConstants.MaxTxCount24H;
        return _normalizationConstants.Clamp(ratio);
    }
}