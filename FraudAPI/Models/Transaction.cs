namespace FraudAPI.Models;

public class Transaction
{
    public double Amount { get; set; }
    public double Installments { get; set; }
    public DateTime RequestedAt { get; set; }
}