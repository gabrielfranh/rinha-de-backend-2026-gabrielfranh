using System.Text.Json.Serialization;

namespace FraudAPI.Dtos;

public record BaseResponseDto (
    [property: JsonPropertyName("approved")] bool Approved, 
    [property: JsonPropertyName("fraud_score")] double FraudScore);    