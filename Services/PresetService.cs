using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services;

public class PresetService
{
    private readonly string _filePath =
        DataPathHelper.GetDataFile(
            "presets.json");

    public List<VoicePreset> LoadPresets()
    {
        if (!File.Exists(_filePath))
            return new();

        string json = File.ReadAllText(_filePath);

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

        File.WriteAllText(_filePath, json);
    }
}