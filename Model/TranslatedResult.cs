using System.Drawing;

namespace ScreenTranslatorApp.Services;

public class TranslatedResult
{
    public string Text { get; set; }
    public string? TranslatedText { get; set; } = null;
    public Rectangle Bounds { get; set; }
}