using DnDVoiceStudio.Models;
using System.IO;

namespace DnDVoiceStudio.Services.Ai;
public class VoiceModelManager
{
    public string RootFolder =>
        DataPathHelper.VoiceModels;

    public void CreateModel(string name)
    {
        string folder =
            Path.Combine(RootFolder, name);

        Directory.CreateDirectory(folder);

        // Always create config (shared metadata for ALL types)
        var config = new VoiceModelConfig
        {
            DisplayName = name,
            Author = "Unknown",
            Description = "New voice model",
            Version = "0.1",
            ModelType = "EMPTY"
        };

        string json =
            System.Text.Json.JsonSerializer.Serialize(
                config,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            Path.Combine(folder, "config.json"),
            json);
    }

    public void DeleteModel(
    string folder)
    {
        if (Directory.Exists(folder))
        {
            Directory.Delete(
                folder,
                true);
        }
    }

    public void DuplicateModel(
        string sourceFolder,
        string newName)
    {
        string destination =
            Path.Combine(
                RootFolder,
                newName);

        CopyDirectory(
            sourceFolder,
            destination);
    }

    private void CopyDirectory(
        string source,
        string destination)
    {
        Directory.CreateDirectory(
            destination);

        foreach (var file in
                 Directory.GetFiles(source))
        {
            File.Copy(
                file,
                Path.Combine(
                    destination,
                    Path.GetFileName(file)),
                true);
        }

        foreach (var dir in
                 Directory.GetDirectories(source))
        {
            CopyDirectory(
                dir,
                Path.Combine(
                    destination,
                    Path.GetFileName(dir)));
        }
    }
    public void CreateModel(string name, string modelType)
    {
        string folder =
            Path.Combine(RootFolder, name);

        Directory.CreateDirectory(folder);

        var config = new VoiceModelConfig
        {
            DisplayName = name,
            Author = "Unknown",
            Description = $"{modelType} voice model",
            Version = "0.1",
            ModelType = modelType
        };

        File.WriteAllText(
            Path.Combine(folder, "config.json"),
            System.Text.Json.JsonSerializer.Serialize(
                config,
                new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                }));

        switch (modelType.ToUpper())
        {
            case "RVC":
                File.Create(Path.Combine(folder, "model.pth")).Dispose();
                File.Create(Path.Combine(folder, "model.index")).Dispose();
                break;

            case "ONNX":
                File.Create(Path.Combine(folder, "model.onnx")).Dispose();
                break;

            case "DVS":
                File.Create(Path.Combine(folder, "model.dvsmodel")).Dispose();
                break;
        }
    }
}