namespace FraudAPI.Models;

public class Merchant : Base
{
    public string Mcc { get; set; }
    public double AvgAmount { get; set; }
}