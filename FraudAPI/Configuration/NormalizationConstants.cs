using System.Text.Json.Serialization;

namespace FraudAPI.Configuration;

public class NormalizationConstants
{
    [JsonPropertyName("max_amount")]
    public int MaxAmount { get; init; }
    
    [JsonPropertyName("max_installments")]
    public int MaxInstallments { get; init; }
    
    [JsonPropertyName("amount_vs_avg_ratio")]
    public int AmountVsAvgRatio { get; init; }
    
    [JsonPropertyName("max_minutes")]
    public int MaxMinutes { get; init; }
    
    [JsonPropertyName("max_km")]
    public int MaxKm { get; init; }
    
    [JsonPropertyName("max_tx_count_24h")]
    public int MaxTxCount24H { get; init; }
    
    [JsonPropertyName("max_merchant_avg_amount")]
    public int MaxMerchantAvgAmount { get; init; }

    public double Clamp(double value)
    {
        return value > 1 ? 1 
            : value < 0 ? 0 
            : value;
    }
}