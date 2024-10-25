namespace PdfGenerator.Interfaces;

public interface IPdfGeneratorService
{
    void CreatePdf<T>(T content, string path) where T : IPdfContent;
}