using System.Configuration;
using System.Data;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace ScreenTranslatorApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private TaskbarIcon _trayIcon;
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        
        _trayIcon = (TaskbarIcon)FindResource("TrayIcon");

        MainWindow = new MainWindow();
        MainWindow.Hide();
        
        _trayIcon.TrayLeftMouseDown += (sender, args) =>
        {
            MainWindow.Show(); 
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        };
    }
    
    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon.Dispose();
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

