using System.Text.Json.Serialization;

namespace FraudAPI.Dtos;

public record FraudRequestDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("transaction")] TransactionDto Transaction,
    [property: JsonPropertyName("customer")] CustomerDto Customer,
    [property: JsonPropertyName("merchant")] MerchantDto Merchant,
    [property: JsonPropertyName("terminal")] TerminalDto Terminal,
    [property: JsonPropertyName("last_transaction")] LastTransactionDto? LastTransaction
    ) : BaseDto(Id);