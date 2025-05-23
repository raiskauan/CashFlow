using System;
using System.Threading.Tasks;

namespace CashFlow.Application.UseCases.Expenses.Reports.PDF;

public interface IGenerateExpansesReportPdfUseCase
{
    Task<byte[]>Execute(DateOnly month);
}