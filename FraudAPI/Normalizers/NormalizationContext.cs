using FraudAPI.Models;

namespace FraudAPI.Normalizers;

public record NormalizationContext
(
    Transaction Transaction,
    Customer Customer,
    Merchant Merchant,
    Terminal Terminal,
    LastTransaction? LastTransaction
);