using System.Text.Json.Serialization;

namespace FraudAPI.Dtos;

public record TerminalDto(
    [property: JsonPropertyName("is_online")] bool IsOnline,
    [property: JsonPropertyName("card_present")] bool CardPresent,
    [property: JsonPropertyName("km_from_home")] double KmFromHome
     );