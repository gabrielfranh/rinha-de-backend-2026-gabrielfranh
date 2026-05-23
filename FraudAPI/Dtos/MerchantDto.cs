using System.Text.Json.Serialization;

namespace FraudAPI.Dtos;

public record MerchantDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("mcc")] string Mcc ,
    [property: JsonPropertyName("avg_amount")] double AvgAmount) : BaseDto(Id);