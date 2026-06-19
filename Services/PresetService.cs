using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services;

public class PresetService
{
    private readonly string _presetPath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
        "Config",
        "presets.json");

    public List<VoicePreset> LoadPresets()
    {
        if (!File.Exists(_presetPath))
            return new();

        string json = File.ReadAllText(_presetPath);

        return JsonSerializer.Deserialize<List<VoicePreset>>(json)
               ?? new();
    }

    public void SavePresets(IEnumerable<VoicePreset> presets)
    {
        string json =
            JsonSerializer.Serialize(
                presets,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(_presetPath, json);
    }
}