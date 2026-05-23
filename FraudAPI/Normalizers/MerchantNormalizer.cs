using FraudAPI.Configuration;

namespace FraudAPI.Normalizers;

public class MerchantNormalizer (NormalizationConstants normalizationConstants, MccRiskTable mccRiskTable)
    : INormalizationStep
{
    private readonly NormalizationConstants _normalizationConstants = normalizationConstants;
    private readonly MccRiskTable _mccRiskTable = mccRiskTable;
    
    public void ExecuteNormalization(NormalizationContext context, NormalizedFeatures features)
    {
        features.UnknownMerchant = NormalizeUnknownMerchant(context.Customer.KnownMerchants, context.Merchant.Id);
        features.MccRisk = NormalizeMccRisk(context.Merchant.Mcc);
        features.MerchantAvgAmount = NormalizeMerchantAvgAmount(context.Merchant.AvgAmount);
    }

    private static double NormalizeUnknownMerchant(string[] knownMerchants, string merchantId) =>
        knownMerchants.Any(merchant => merchant == merchantId) ? 0 : 1;

    private double NormalizeMccRisk(string mcc) =>
        _mccRiskTable.GetRisk(mcc);

    private double NormalizeMerchantAvgAmount(double merchantAvgAmount)
    {
        var ratio = merchantAvgAmount / _normalizationConstants.MaxMerchantAvgAmount;
        return _normalizationConstants.Clamp(ratio);
    }
}