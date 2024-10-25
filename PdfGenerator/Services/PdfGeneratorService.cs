using PdfGenerator.Interfaces;
using PdfGenerator.Models;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGenerator.Services;

public class PdfGeneratorService : IPdfGeneratorService
{
    private readonly PdfGeneratorOptions _options;

    public PdfGeneratorService(PdfGeneratorOptions? options = null)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        _options = options ?? new PdfGeneratorOptions();
    }

    public void CreatePdf<T>(T content, string path) where T : IPdfContent
    {
        Document.Create(container => GenerateDocument(container, content))
                .GeneratePdf(path);
        //.ShowInCompanion();
    }

    private void GenerateDocument<T>(IDocumentContainer container, T content) where T : IPdfContent
    {
        container.Page(page =>
        {
            ConfigurePage(page);
            GenerateHeader(page.Header(), content);
            GenerateContent(page.Content(), content);
            GenerateFooter(page.Footer(), content);
        });
    }

    private void ConfigurePage(PageDescriptor page)
    {
        page.Size(PageSizes.A4.Landscape());
        page.PageColor(Colors.White);
        page.MarginVertical(_options.VerticalMargin, Unit.Centimetre);
        page.MarginHorizontal(_options.HorizontalMargin, Unit.Centimetre);
    }

    private void GenerateHeader<T>(IContainer header, T content) where T : IPdfContent
    {
        header.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Cell().Column(1).AlignLeft().PaddingLeft(40).Width(50)
                 .Image(Placeholders.Image(50, 50));
            //.Image(content.LogoPath);

            table.Cell().Column(2).AlignCenter()
                 .Text(content.Header)
                 .FontSize(_options.BaseFontSize)
                 .Bold();

            table.Cell().Column(3).AlignRight().PaddingRight(40).Width(50)
                 //.Image(content.LogoPath);
                 .Image(Placeholders.Image(50, 50));
        });
    }

    private void GenerateContent<T>(IContainer container, T content) where T : IPdfContent
    {
        container.PaddingTop(15).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(1);
                columns.RelativeColumn(10);
            });

            for (int i = 0; i < content.Data?.Count; i++)
            {
                GenerateRow(table, i, content.Data[i]);
            }
        });
    }

    private void GenerateRow(TableDescriptor table, int index, string text)
    {
        var rowIndex = (uint)index + 1;

        table.Cell().Column(1).Row(rowIndex)
             .Border(0.5f)
             .BorderColor(Colors.Black)
             .Height(45)
             .AlignCenter()
             .AlignMiddle()
             .Text($"{index + 1}.")
             .FontFamily("Arial")
             .FontSize(30)
             .FontColor(Colors.Black)
             .Bold();

        table.Cell().Column(2).Row(rowIndex)
             .Border(0.5f)
             .BorderColor(Colors.Black)
             .Height(45)
             .AlignMiddle()
             .PaddingLeft(10)
             .Text(text)
             .FontFamily("Arial")
             .FontSize(CalculateFontSize(text))
             .FontColor(Colors.Black)
             .Bold();
    }

    private void GenerateFooter(IContainer footer, IPdfContent content)
    {
        footer.AlignCenter()
              .Width(200)
              .Height(50)
              //.Image(content.FooterImagePath);
              .Image(Placeholders.Image(200, 50));
    }

    private float CalculateFontSize(string text)
    {
        if (text.Length <= _options.MaxTextLength)
            return _options.BaseFontSize;

        var lengthDifference = text.Length - _options.MaxTextLength;
        var scalingFactor = DetermineScalingFactor(text.Length);
        var fontSize = _options.BaseFontSize - (lengthDifference * scalingFactor);
        Console.WriteLine($"{text} font size: {fontSize}");
        return Math.Max(fontSize, _options.MinFontSize);
    }

    private float DetermineScalingFactor(int textLength)
    {
        var thresholds = _options.GetScalingThresholds()
                                .OrderBy(t => t.Length);

        foreach (var threshold in thresholds)
        {
            if (textLength <= threshold.Length)
            {
                return threshold.Factor;
            }
        }

        // En son threshold değerini kullan
        return thresholds.Last().Factor;
    }
}