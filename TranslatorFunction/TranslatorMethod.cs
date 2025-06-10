using ScreenTranslatorApp.Services;

namespace ScreenTranslatorApp.TranslatorFunction;

public static class TranslatorMethod
{
    public static async Task TranslatorMethod_1(OcrEngine engine, TranslationService translatorMethod, CancellationToken cancellationToken)
    {
        try
        {
            var screen = ScreenCapturer.CaptureFullScreen();
            /*
            screen.Save("screenshot.png", System.Drawing.Imaging.ImageFormat.Png);
            */
            var listOfSentence = engine.Recognize(screen);
            foreach (var sentence in listOfSentence)
            {
                await translatorMethod.TranslateAsync(sentence, cancellationToken);
            }

            var overlay = new OverlayWindow();
            overlay.ShowTranslatedResults(listOfSentence);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public static void TranslatorCancel(OverlayWindow overlay)
    {
        overlay.ClearOverlay();
    }
}