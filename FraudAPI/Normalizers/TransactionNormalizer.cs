using FraudAPI.Configuration;

namespace FraudAPI.Normalizers;

public class TransactionNormalizer(NormalizationConstants normalizationConstants)
    : INormalizationStep
{
    private readonly NormalizationConstants _normalizationConstants = normalizationConstants;
    
    public void ExecuteNormalization(NormalizationContext context, NormalizedFeatures features)
    {
        features.Amount    = NormalizeAmount(context.Transaction.Amount);
        features.Installments = NormalizeInstallments(context.Transaction.Installments);
        features.AmountVsAvg = NormalizeAmountVsAvg(context.Transaction.Amount, context.Customer.AvgAmount);
        features.HourOfDay = NormalizeHourOfDay(context.Transaction.RequestedAt);
        features.DayOfWeek = NormalizeDayOfWeek(context.Transaction.RequestedAt);
    }

    private double NormalizeAmount(double amount)
    {
        var ratio = amount/_normalizationConstants.MaxAmount;
        return _normalizationConstants.Clamp(ratio);
    }

    private double NormalizeInstallments(double installments)
    {
        var ratio = installments / _normalizationConstants.MaxInstallments;
        return _normalizationConstants.Clamp(ratio);
    }

    private double NormalizeAmountVsAvg(double amount, double amountVsAvg)
    {
        var ratio = (amount / amountVsAvg) / _normalizationConstants.AmountVsAvgRatio;
        return _normalizationConstants.Clamp(ratio);
    }

    private double NormalizeHourOfDay(DateTime requestedAt)
    {
        var ratio = requestedAt.ToUniversalTime().Hour / 23.0;
        return _normalizationConstants.Clamp(ratio);
    }

    private double NormalizeDayOfWeek(DateTime requestedAt)
    {
        var day = (int)requestedAt.ToUniversalTime().DayOfWeek;
        var adjusted = (day + 6) % 7;
        return normalizationConstants.Clamp(adjusted / 6.0);
    }
}