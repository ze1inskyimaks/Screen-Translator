using System.Windows;
using System.Windows.Input;
using Hardcodet.Wpf.TaskbarNotification;
using NHotkey;
using NHotkey.Wpf;
using ScreenTranslatorApp.Properties;
using ScreenTranslatorApp.Services;
using ScreenTranslatorApp.TranslatorFunction;

namespace ScreenTranslatorApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon _trayIcon;
    private AppConfig _config;
    private OcrEngine _engine;
    private TranslationService _translationService;
    public static CancellationTokenSource CancellationTokenSource { get; private set; }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        _config = AppConfig.Load();
        _trayIcon = (TaskbarIcon)FindResource("TrayIcon") ?? throw new InvalidOperationException();
        _engine = new OcrEngine();
        _translationService = new TranslationService();
        CancellationTokenSource = new CancellationTokenSource();

        MainWindow = new MainWindow();
        MainWindow.Hide();
        
        _trayIcon.TrayLeftMouseDown += (_, _) =>
        {
            MainWindow.Show(); 
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        };
        //Method 1
        HotkeyManager.Current.AddOrReplace("TranslateHotkeyMethod", _config.GetKey(_config.HotkeyForMethod1), _config.GetModifiers(_config.HotkeyForMethod1), OnHotkeyMethod1Pressed);
        //Cancel
        HotkeyManager.Current.AddOrReplace("TranslateHotkeyCancel", _config.GetKey(_config.HotkeyForCancel), _config.GetModifiers(_config.HotkeyForCancel), OnHotkeyCancelPressed);
    }

    private async void OnHotkeyMethod1Pressed(object? sender, HotkeyEventArgs e)
    {
        _trayIcon.ShowBalloonTip("Translator!", $"Гаряча клавіша {_config.HotkeyForMethod1} натиснута!", BalloonIcon.Info);
        await TranslatorMethod.TranslatorMethod_1(_engine, _translationService, CancellationTokenSource.Token);
        e.Handled = true;
    }
    
    private async void OnHotkeyCancelPressed(object? sender, HotkeyEventArgs e)
    {
        _trayIcon.ShowBalloonTip("Translator!", $"Гаряча клавіша {_config.HotkeyForCancel} натиснута!", BalloonIcon.Info);
        TranslatorMethod.TranslatorCancel();
        e.Handled = true;
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon.Dispose();
        CancellationTokenSource?.Cancel();
        base.OnExit(e);
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        if (MainWindow != null)
        {
            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        _trayIcon.Dispose();
        Current.Shutdown();
    }
}

