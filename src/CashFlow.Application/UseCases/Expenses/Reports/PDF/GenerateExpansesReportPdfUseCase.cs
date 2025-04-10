using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Extensions;
using CashFlow.Application.UseCases.Expenses.Reports.PDF.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.PDF.Fonts;
using CashFlow.Domain.Repositories.Expanses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.PDF;

public class GenerateExpansesReportPdfUseCase : IGenerateExpansesReportPdfUseCase
{
    private const string CURRENT_SYMBOL = "€";
    private const int HEIGHT_ROW_EXPANSE_TABLE = 25;
    
    private readonly IExpansesReadOnlyRepository _repository;
    
    public GenerateExpansesReportPdfUseCase(IExpansesReadOnlyRepository repository)
    {
        _repository = repository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }


    public async Task<byte[]> Execute(DateOnly month)
    {
        var expanses = await _repository.FilterByMonth(month);
        if (expanses.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);
        
        CreateHeaderWithProfilePhotoAndName(page);

        var totalExpenses = expanses.Sum(exp => exp.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expanses)
        {
            var table = CreateExpanseTable(page);
            
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPANSE_TABLE;
            
            AddExpanseTitle(row.Cells[0],expense.Title);
            AddHeaderForAmount(row.Cells[3]);
            
            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPANSE_TABLE;
            
            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpanseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;
            
            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpanseInformation(row.Cells[1]);
            
            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpanseInformation(row.Cells[2]);

            AddAmountForExpanse(row.Cells[3], expense.Amount);

            // Se essa verificação for igual a Falsa ela vai devolver o campo de descrição
            // caso o contrario não haverá descrição.
            
            if (string.IsNullOrWhiteSpace(expense.Description) == false)
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_EXPANSE_TABLE;

                descriptionRow.Cells[0].AddParagraph(expense.Description);
                descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
                descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }
            
            AddWhiteSpace(table);
        }
        
        return RenderDocument(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessage.EXPANSES_FOR} {month.ToString("Y")}";
        document.Info.Author = "Kauan Rais";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.RightMargin = 80;
        
        return section;
    }

    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();
        
        var assembly = Assembly.GetExecutingAssembly();
        var location = assembly.Location;
        var directoryName = Path.GetDirectoryName(location);
        var pathFile = Path.Combine(directoryName!, "Logo", "ProfilePhoto.jpg"); 

        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph("Hey, Kauan Rais");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";
        
        var title = string.Format(ResourceReportGenerationMessage.TOTAL_SPENT_ON, month.ToString("Y"));
        
        paragraph.AddFormattedText(title, new Font {Name = FontHelper.RALEWAY_REGULAR, Size = 15});
        
        paragraph.AddLineBreak();

        
        paragraph.AddFormattedText($"{totalExpenses} {CURRENT_SYMBOL}", new Font {Name = FontHelper.WORKSANS_BLACK, Size = 50});

    }

    private Table CreateExpanseTable(Section page)
    {
        var table = page.AddTable();
        
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;
        
        return table;
    }

    private void AddExpanseTitle(Cell cell, string expensesTitle)
    {
        cell.AddParagraph(expensesTitle);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessage.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpanseInformation(Cell cell)
    {
        cell.Format.Font = new Font() { Name = FontHelper.WORKSANS_REGULAR, Size = 12 , Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddAmountForExpanse(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{amount.ToString(CultureInfo.InvariantCulture)} {CURRENT_SYMBOL}");
        cell.Format.Font = new Font {Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer()
        {
            Document = document
        };
        
        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);
        return file.ToArray();
    }

    
}