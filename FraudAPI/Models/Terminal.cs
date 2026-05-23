namespace FraudAPI.Models;

public class Terminal
{
    public bool IsOnline { get; set; }
    public bool CardPresent { get; set; }
    public double KmFromHome { get; set; }
}