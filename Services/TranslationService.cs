using System.Net.Http;
using System.Text;
using System.Text.Json;
using ScreenTranslatorApp.Properties;

namespace ScreenTranslatorApp.Services;

public class TranslationService
{
    private readonly HttpClient _httpClient;
    private AppConfig _config;
    private string _targetLang;
    private string _baseUrl = "https://api-free.deepl.com/v2/translate";
    private string _apiKey = "6a399a85-8b2a-415f-a15b-6e6671d28257:fx";

    public TranslationService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        _config = AppConfig.Load();
        _targetLang = _config.TargetLanguage;
    }

    public async Task<TranslatedResult> TranslateAsync(TranslatedResult translatedResult, CancellationToken cancellationToken, string? targetLang = null, string sourceLang = "auto")
    {
        if (targetLang is null)
        {
            targetLang = _targetLang;
        }
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("auth_key", _apiKey),
            new KeyValuePair<string, string>("text", translatedResult.Text),
            new KeyValuePair<string, string>("target_lang", targetLang.ToUpper())
        });

        var response = await _httpClient.PostAsync(_baseUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<DeepLResponse>(json, options);

        translatedResult.TranslatedText = result?.Translations?.FirstOrDefault()?.Text ?? "";

        return translatedResult;
    }
    
    public class DeepLResponse
    {
        public List<TranslationItem> Translations { get; set; } = new();
    }

    public class TranslationItem
    {
        public string Text { get; set; } = "";
        public string DetectedSourceLanguage { get; set; } = "";
    }
}