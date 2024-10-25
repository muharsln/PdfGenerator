namespace PdfGenerator.Models;

public record PdfGeneratorOptions(
    float BaseFontSize = 30,
    float MinFontSize = 15,
    float MaxTextLength = 30,
    float HorizontalMargin = 2f,
    float VerticalMargin = 2f,
    IReadOnlyList<ScalingThreshold>? ScalingThresholds = null
)
{
    public IReadOnlyList<ScalingThreshold> GetScalingThresholds() => ScalingThresholds ?? DefaultScalingThresholds;

    private static readonly IReadOnlyList<ScalingThreshold> DefaultScalingThresholds = new List<ScalingThreshold>
    {
        new(40, 1.1f),   // 40 karaktere kadar normal scaling
        new(50, 1.2f),   // 41-50 karakter arası orta seviye scaling
        new(70, 1.3f),   // 61-70 karakter arası orta seviye scaling
        new(80, 1.4f),   // 61-70 karakter arası orta seviye scaling
        new(100, 1.5f),   // 61-70 karakter arası orta seviye scaling
        new(120, 2.0f),   // Çok uzun metinler için
        new(int.MaxValue, 2.5f),   // 80 karakterden sonra çok agresif scaling
    };
}