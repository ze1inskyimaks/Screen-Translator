using ScreenTranslatorApp.Services;

namespace ScreenTranslatorApp.TranslatorFunction;

public static class TranslatorMethod
{
    public static async Task TranslatorMethod_1(OcrEngine engine, TranslationService translatorMethod, CancellationToken cancellationToken)
    {
        var loading = new LoadingWindow();
        loading.Show();

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

            OverlayWindow.Instance.ShowTranslatedResults(listOfSentence);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            loading.Close();
        }
    }
    
    public static void TranslatorCancel()
    {
        OverlayWindow.Instance.ClearOverlay();
    }
}