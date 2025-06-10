using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace ScreenTranslatorApp.Properties;

public class AppConfig
{
    public string HotkeyForMethod1 { get; set; } = "Shift+T";
    public string HotkeyForCancel { get; set; } = "Shift+D";
    public string TargetLanguage { get; set; } = "en";

    private const string ConfigPath = "config.json";

    public static AppConfig Load()
    {
        if (!File.Exists(ConfigPath))
            return new AppConfig();

        var json = File.ReadAllText(ConfigPath);
        return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(ConfigPath, json);
    }
    
    public KeyGesture? GetKeyGesture(string hotkey)
    {
        try
        {
            var converter = new KeyGestureConverter();
            return (KeyGesture?)converter.ConvertFromString(hotkey);
        }
        catch
        {
            return null;
        }
    }
    
    public ModifierKeys GetModifiers(string hotkey)
    {
        var gesture = GetKeyGesture(hotkey);
        return gesture?.Modifiers ?? ModifierKeys.None;
    }

    public Key GetKey(string hotkey)
    {
        var gesture = GetKeyGesture(hotkey);
        return gesture?.Key ?? Key.None;
    }

}
