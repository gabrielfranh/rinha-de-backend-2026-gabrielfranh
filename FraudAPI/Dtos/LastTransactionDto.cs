using System.Text.Json.Serialization;
public record LastTransactionDto(
    [property: JsonPropertyName("timestamp")] DateTime Timestamp,
    [property: JsonPropertyName("km_from_current")] double KmFromCurrent);