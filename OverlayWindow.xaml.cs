using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ScreenTranslatorApp.Services;

namespace ScreenTranslatorApp;

public partial class OverlayWindow : Window
{
    public OverlayWindow()
    {
        InitializeComponent();
        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;
        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;
    }

    public void ShowTranslatedResults(IEnumerable<TranslatedResult> results)
    {
        OverlayCanvas.Children.Clear();

        foreach (var result in results)
        {
            var textBlock = new TextBlock
            {
                Text = result.TranslatedText ?? result.Text,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromArgb(180, 0, 0, 0)),
                FontSize = 16,
                Padding = new Thickness(4),
                TextWrapping = TextWrapping.Wrap
            };

            Canvas.SetLeft(textBlock, result.Bounds.X);
            Canvas.SetTop(textBlock, result.Bounds.Y);

            OverlayCanvas.Children.Add(textBlock);
        }

        Show();
    }

    public void ClearOverlay()
    {
        OverlayCanvas.Children.Clear();
        Hide();
    }
}