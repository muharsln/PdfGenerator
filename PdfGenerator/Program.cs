using PdfGenerator.Models;
using PdfGenerator.Services;

var options = new PdfGeneratorOptions(
    BaseFontSize: 30,
    MinFontSize: 10,
    ScalingThresholds: new List<ScalingThreshold>()
    {
        new(40, 1.1f),   // 40 karaktere kadar scaling
        new(50, 1.2f),   // 41-50 karakter arası seviye scaling
        new(70, 1.3f),   // 51-70 karakter arası seviye scaling
        new(80, 1.4f),   // 71-80 karakter arası seviye scaling
        new(100, 1.5f),   // 81-100 karakter arası seviye scaling
        new(int.MaxValue, 2.5f),   // Çok uzun metinler için
    }
);

var service = new PdfGeneratorService(options);

// Content oluşturma
var content = new PdfContent(
    Header: "Başlık",
    Data: new List<string> {
    "Muhammed Arslan",
    "Muhammed Arslan Muhammed Arslan",
    "Muhammed Arslan Muhammed Arslan Muhammed Arslan",
    "Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan",
    "Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan",
    "Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan",
    "Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan Muhammed Arslan"
    },
    LogoPath: "logo.png",
    FooterImagePath: "footer.png",
    SavePath: "C:\\Users\\Arslan\\Desktop\\AutoTest.pdf"
);

service.CreatePdf(content, content.SavePath);