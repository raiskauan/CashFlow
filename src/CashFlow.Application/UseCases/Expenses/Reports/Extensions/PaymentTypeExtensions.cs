using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Domain.Enums;

namespace CashFlow.Application.UseCases.Expenses.Reports.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportGenerationMessage.CASH,
            PaymentType.CreditCard => ResourceReportGenerationMessage.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportGenerationMessage.DEBIT_CARD,
            PaymentType.ElectronicTransfer => ResourceReportGenerationMessage.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
    
}