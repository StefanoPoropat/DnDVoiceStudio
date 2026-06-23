using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services.Ai;

public static class VoiceModelConfigLoader
{
    public static VoiceModelConfig
        Load(string modelFolder)
    {
        try
        {
            string path =
                Path.Combine(
                    modelFolder,
                    "config.json");

            if (!File.Exists(path))
            {
                return new VoiceModelConfig();
            }

            string json =
                File.ReadAllText(path);

            return JsonSerializer.Deserialize
                <VoiceModelConfig>(json)
                   ?? new VoiceModelConfig();
        }
        catch
        {
            return new VoiceModelConfig();
        }
    }
}