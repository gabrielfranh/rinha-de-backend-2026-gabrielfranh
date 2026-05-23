namespace FraudAPI.Normalizers;

public class NormalizedFeatures
{
    public double Amount { get; set; }
    public double Installments { get; set; }
    public double AmountVsAvg { get; set; }
    public double HourOfDay { get; set; }
    public double DayOfWeek { get; set; }
    public double MinutesSinceLastTx { get; set; }
    public double KmFromLastTx { get; set; }
    public double KmFromHome { get; set; }
    public double TxCount24h { get; set; }
    public double IsOnline { get; set; }
    public double CardPresent { get; set; }
    public double UnknownMerchant { get; set; }
    public double MccRisk { get; set; }
    public double MerchantAvgAmount { get; set; }

    public double[] ToOrderedArray() =>
    [
        Amount,
        Installments,
        AmountVsAvg,
        HourOfDay,
        DayOfWeek,
        MinutesSinceLastTx,
        KmFromLastTx,
        KmFromHome,
        TxCount24h,
        IsOnline,
        CardPresent,
        UnknownMerchant,
        MccRisk,
        MerchantAvgAmount
    ];
}