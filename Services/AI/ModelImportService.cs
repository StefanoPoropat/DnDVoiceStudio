using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services.Ai;

public class ModelImportService
{
    private readonly string _root =
        DataPathHelper.VoiceModels;

    public IAiModel? ImportFolder(
        string sourceFolder)
    {
        if (!Directory.Exists(sourceFolder))
            return null;

        string modelName =
            Path.GetFileName(sourceFolder);

        string targetFolder =
            Path.Combine(
                _root,
                modelName);

        Directory.CreateDirectory(_root);

        // Prevent overwrite
        if (Directory.Exists(targetFolder))
        {
            targetFolder =
                Path.Combine(
                    _root,
                    modelName + "_" +
                    Guid.NewGuid().ToString("N")[..6]);
        }

        Directory.CreateDirectory(targetFolder);

        // Copy everything
        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            string dest =
                Path.Combine(
                    targetFolder,
                    Path.GetFileName(file));

            File.Copy(file, dest, true);
        }

        // Detect model type
        IAiModel model =
            DetectModel(targetFolder, modelName);

        // Ensure config exists
        EnsureConfig(model, targetFolder);

        return model;
    }

    private IAiModel DetectModel(
        string folder,
        string name)
    {
        if (Directory.GetFiles(folder, "*.pth").Any())
            return new RvcModel
            {
                Name = name,
                FolderPath = folder
            };

        if (Directory.GetFiles(folder, "*.onnx").Any())
            return new OnnxModel
            {
                Name = name,
                FolderPath = folder
            };

        if (Directory.GetFiles(folder, "*.dvsmodel").Any())
            return new DvsModel
            {
                Name = name,
                FolderPath = folder
            };

        return new EmptyAiModel
        {
            Name = name,
            FolderPath = folder
        };
    }

    private void EnsureConfig(
        IAiModel model,
        string folder)
    {
        string path =
            Path.Combine(
                folder,
                "config.json");

        if (File.Exists(path))
            return;

        var config =
            new VoiceModelConfig
            {
                DisplayName = model.Name
            };

        string json =
            JsonSerializer.Serialize(
                config,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(path, json);
    }
}