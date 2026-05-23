using FraudAPI.Dtos;
using FraudAPI.Models;

namespace FraudAPI.Utils;

public static class FraudHelper
{
    public static FraudRequest ToModel(FraudRequestDto fraudRequestDto)
    {
        var transaction = new Transaction()
        {
            Amount = fraudRequestDto.Transaction.Amount,
            Installments = fraudRequestDto.Transaction.Installments,
            RequestedAt = fraudRequestDto.Transaction.RequestedAt,
        };
        
        var customer = new Customer()
        {
            AvgAmount = fraudRequestDto.Customer.AvgAmount,
            KnownMerchants = fraudRequestDto.Customer.KnownMerchants,
            TxCount24H = fraudRequestDto.Customer.TxCount24H,
        };

        var merchant = new Merchant()
        {
            AvgAmount = fraudRequestDto.Merchant.AvgAmount,
            Mcc = fraudRequestDto.Merchant.Mcc,
            Id = fraudRequestDto.Merchant.Id,
        };

        var terminal = new Terminal()
        {
            CardPresent = fraudRequestDto.Terminal.CardPresent,
            IsOnline = fraudRequestDto.Terminal.IsOnline,
            KmFromHome = fraudRequestDto.Terminal.KmFromHome,
        };

        var lastTransaction = 
            fraudRequestDto.LastTransaction is null ? null 
                : new LastTransaction()
        {
            KmFromCurrent = fraudRequestDto.LastTransaction.KmFromCurrent,
            Timestamp = fraudRequestDto.LastTransaction.Timestamp,
        };

        return new FraudRequest()
        {
            Id = fraudRequestDto.Id,
            Transaction = transaction,
            Customer = customer,
            Merchant = merchant,
            Terminal = terminal,
            LastTransaction = lastTransaction
        };
    }
}