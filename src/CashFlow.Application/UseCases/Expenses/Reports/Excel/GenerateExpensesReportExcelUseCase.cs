using System.Globalization;
using CashFlow.Application.UseCases.Expenses.Reports.Extensions;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories.Expanses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "$";

    private readonly IExpansesReadOnlyRepository _repository;

    public GenerateExpensesReportExcelUseCase(IExpansesReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expanses = await _repository.FilterByMonth(month);
        if (expanses.Count == 0)
        {
            return [];
        }

        using var workbook = new XLWorkbook();

        workbook.Author = "Kauan Rais";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Calibre";

        month.ToString("Y");

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(worksheet);

        var raw = 2;
        foreach (var expense in expanses)
        {
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date;
            worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();
            worksheet.Cell($"D{raw}").Value = expense.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
            worksheet.Cell($"E{raw}").Value = expense.Description;

            raw++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();

        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessage.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessage.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessage.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessage.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessage.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#ADD8E6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}