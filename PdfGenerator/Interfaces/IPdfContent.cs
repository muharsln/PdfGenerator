namespace PdfGenerator.Interfaces;

public interface IPdfContent
{
    string Header { get; }
    IReadOnlyList<string> Data { get; }
    string LogoPath { get; }
    string FooterImagePath { get; }
    string SavePath { get; }
}