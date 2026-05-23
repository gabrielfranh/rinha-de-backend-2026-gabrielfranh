using System.Text.Json.Serialization;

namespace FraudAPI.Dtos;

public record TransactionDto(
    [property: JsonPropertyName("amount")] double Amount,
    [property: JsonPropertyName("installments")] int Installments,
    [property: JsonPropertyName("requested_at")] DateTime RequestedAt
    );