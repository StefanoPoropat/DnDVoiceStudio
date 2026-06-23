using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services.Ai;

public class VoiceModelService
{
    public List<AiModelInfo> LoadModels()
    {
        List<AiModelInfo> models =
            new();

        string root =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "VoiceModels");

        if (!Directory.Exists(root))
            return models;

        foreach (string folder in
                 Directory.GetDirectories(root))
        {
            string configPath =
                Path.Combine(folder,
                    "config.json");

            string modelPath =
                Path.Combine(folder,
                    "model.onnx");

            if (!File.Exists(modelPath))
                continue;

            VoiceModelConfig config =
                new();

            if (File.Exists(configPath))
            {
                config =
                    JsonSerializer.Deserialize<VoiceModelConfig>(
                        File.ReadAllText(configPath))
                    ?? new();
            }

            models.Add(
                new AiModelInfo
                {
                    Name = config.Name,
                    Folder = folder,
                    ModelPath = modelPath,
                    PortraitPath =
                        Path.Combine(
                            folder,
                            "portrait.png"),

                    Config = config
                });
        }

        return models;
    }
}