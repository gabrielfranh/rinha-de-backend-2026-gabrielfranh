namespace FraudAPI.Models;

public class FraudRequest
{
    public required string Id { get; set; }
    public required Transaction Transaction { get; set; }
    public required Customer Customer { get; set; }
    public required Merchant Merchant { get; set; }
    public required Terminal Terminal { get; set; }
    public required LastTransaction? LastTransaction { get; set; }
}