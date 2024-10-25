using PdfGenerator.Interfaces;

namespace PdfGenerator.Models;

public record PdfContent(
    string Header,
    IReadOnlyList<string> Data,
    string LogoPath,
    string FooterImagePath,
    string SavePath
) : IPdfContent;