using FraudAPI.Configuration;
using FraudAPI.Models;

namespace FraudAPI.Normalizers;

public class LastTransactionNormalizer(NormalizationConstants normalizationConstants)
    : INormalizationStep
{
    private readonly NormalizationConstants _normalizationConstants = normalizationConstants;

    public void ExecuteNormalization(NormalizationContext context, NormalizedFeatures features)
    {
        features.MinutesSinceLastTx = NormalizeMinutesSinceLastTx(context.Transaction, context.LastTransaction);
        features.KmFromLastTx = NormalizeKmFromLastTx(context.LastTransaction);
    }

    private double NormalizeMinutesSinceLastTx(Transaction transaction, LastTransaction? lastTransaction)
    {
        if (lastTransaction is null)
            return -1;
        
        var minutes = (transaction.RequestedAt - lastTransaction.Timestamp.ToUniversalTime()).TotalMinutes;
        var ratio = minutes / normalizationConstants.MaxMinutes;
        return normalizationConstants.Clamp(ratio);
    }

    private double NormalizeKmFromLastTx(LastTransaction? lastTransaction)
    {
        if (lastTransaction is null)
            return -1;

        var ratio = lastTransaction.KmFromCurrent / _normalizationConstants.MaxKm;
        return normalizationConstants.Clamp(ratio);
    }
}