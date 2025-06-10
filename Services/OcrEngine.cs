using System.Drawing;
using Tesseract;

namespace ScreenTranslatorApp.Services;

public class OcrEngine
{
    private readonly TesseractEngine _engine;

    public OcrEngine(string language = "eng")
    {
        _engine = new TesseractEngine(datapath: "C:\\Storage\\BackEnd\\ScreenTranslatorApp\\tessdata\\", language, EngineMode.Default);
    }

    public List<TranslatedResult> Recognize(Bitmap image)
    {
        var results = new List<TranslatedResult>();

        using var pix = PixConverter.ToPix(image);
        using var page = _engine.Process(pix);
        using var iter = page.GetIterator();

        iter.Begin();
        do
        {
            if (iter.TryGetBoundingBox(PageIteratorLevel.TextLine, out var rect))
            {
                string text = iter.GetText(PageIteratorLevel.TextLine);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    results.Add(new TranslatedResult
                    {
                        Text = text.Trim(),
                        Bounds = new Rectangle(rect.X1, rect.Y1, rect.Width, rect.Height)
                    });
                }
            }
        }
        while (iter.Next(PageIteratorLevel.TextLine));

        return results;
    }
}