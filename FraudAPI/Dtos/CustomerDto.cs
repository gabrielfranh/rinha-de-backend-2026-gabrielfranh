using System.Text.Json.Serialization;

namespace FraudAPI.Dtos;

public record CustomerDto(
    [property: JsonPropertyName("avg_amount")] double AvgAmount,
    [property: JsonPropertyName("tx_count_24h")] int TxCount24H,
    [property: JsonPropertyName("known_merchants")] string[] KnownMerchants);