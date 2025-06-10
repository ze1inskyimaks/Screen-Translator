using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ScreenTranslatorApp.Services;

namespace ScreenTranslatorApp;

public partial class OverlayWindow : Window
{
    private static OverlayWindow _instance;

    public static OverlayWindow Instance
    {
        get
        {
            if (_instance == null)
                _instance = new OverlayWindow();
            return _instance;
        }
    }

    private OverlayWindow()
    {
        InitializeComponent();

        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;
        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;

        WindowStyle = WindowStyle.None;
        AllowsTransparency = true;
        Background = Brushes.Transparent;
        Topmost = true;
        ShowInTaskbar = false;
    }

    public void ShowTranslatedResults(IEnumerable<TranslatedResult> results)
    {
        if (OverlayCanvas == null) return;

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
                TextWrapping = TextWrapping.Wrap,
                MaxWidth = 400 // Обмеження ширини для зручного читання
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