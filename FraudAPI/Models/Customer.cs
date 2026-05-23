namespace FraudAPI.Models;

public class Customer
{
    public double AvgAmount { get; set; }
    public double TxCount24H { get; set; }
    public string[] KnownMerchants { get; set; }
}