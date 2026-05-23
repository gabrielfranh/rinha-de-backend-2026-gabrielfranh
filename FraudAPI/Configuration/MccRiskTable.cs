using System.Text.Json;

namespace FraudAPI.Configuration;

public class MccRiskTable
{
    private readonly Dictionary<string, double> _table;
    private const double DefaultRisk = 0.5;

    public MccRiskTable(string jsonPath)
    {
        var json = File.ReadAllText(jsonPath);
        _table = JsonSerializer.Deserialize<Dictionary<string, double>>(json) ?? [];
    }

    public double GetRisk(string mcc) =>
        _table.GetValueOrDefault(mcc, DefaultRisk);
}