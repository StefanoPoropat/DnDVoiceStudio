using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services.Ai;

public static class VoiceModelConfigSaver
{
    public static void Save(
        string modelFolder,
        VoiceModelConfig config)
    {
        string path =
            Path.Combine(
                modelFolder,
                "config.json");

        string json =
            JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            path,
            json);
    }
}