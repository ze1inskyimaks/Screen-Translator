using System.IO;
using System.Text.Json;

namespace ScreenTranslatorApp.Properties;

public class AppConfig
{
    public string Hotkey { get; set; } = "Ctrl+Shift+T";
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
}
