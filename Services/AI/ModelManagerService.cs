using DnDVoiceStudio.Models;
using DnDVoiceStudio.Services.AI.DVS;
using DnDVoiceStudio.Services.AI.Interfaces;
using DnDVoiceStudio.Services.AI.ONNX;
using DnDVoiceStudio.Services.AI.RVC;
using System.IO;
using System.IO.Compression;
using System.Text.Json;

namespace DnDVoiceStudio.Services.Ai;

public class ModelManagerService
{
    private readonly string _root =
        DataPathHelper.VoiceModels;

    // ----------------------------
    // RENAME MODEL
    // ----------------------------
    public void RenameModel(
        IAiModel model,
        string newName)
    {
        string newFolder =
            Path.Combine(_root, newName);

        if (Directory.Exists(newFolder))
            throw new Exception("Model already exists");

        Directory.Move(
            model.FolderPath,
            newFolder);

        UpdateConfigName(newFolder, newName);
    }

    // ----------------------------
    // DELETE MODEL
    // ----------------------------
    public void DeleteModel(
        IAiModel model)
    {
        if (Directory.Exists(model.FolderPath))
            Directory.Delete(
                model.FolderPath,
                true);
    }

    // ----------------------------
    // DUPLICATE MODEL
    // ----------------------------
    public IAiModel DuplicateModel(
        IAiModel model,
        string newName)
    {
        string newFolder =
            Path.Combine(_root, newName);

        CopyDirectory(
            model.FolderPath,
            newFolder);

        UpdateConfigName(newFolder, newName);

        return new EmptyAiModel
        {
            Name = newName,
            FolderPath = newFolder
        };
    }

    // ----------------------------
    // EXPORT MODEL (.zip)
    // ----------------------------
    public string ExportModel(
        IAiModel model,
        string exportPath)
    {
        string zipPath =
            Path.Combine(
                exportPath,
                $"{model.Name}.zip");

        if (File.Exists(zipPath))
            File.Delete(zipPath);

        ZipFile.CreateFromDirectory(
            model.FolderPath,
            zipPath);

        return zipPath;
    }

    // ----------------------------
    // IMPORT MODEL PACK (.zip)
    // ----------------------------
    public IAiModel ImportZip(
        string zipFile)
    {
        string extractFolder =
            Path.Combine(
                _root,
                Path.GetFileNameWithoutExtension(zipFile));

        ZipFile.ExtractToDirectory(
            zipFile,
            extractFolder,
            true);

        return DetectModel(extractFolder);
    }

    // ----------------------------
    // CONFIG UPDATE
    // ----------------------------
    private void UpdateConfigName(
        string folder,
        string name)
    {
        string path =
            Path.Combine(folder, "config.json");

        VoiceModelConfig config =
            File.Exists(path)
                ? JsonSerializer.Deserialize<VoiceModelConfig>(
                    File.ReadAllText(path))
                  ?? new VoiceModelConfig()
                : new VoiceModelConfig();

        config.DisplayName = name;

        File.WriteAllText(
            path,
            JsonSerializer.Serialize(config,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
    }

    // ----------------------------
    // DETECT MODEL TYPE
    // ----------------------------
    private IAiModel DetectModel(string folder)
    {
        if (Directory.GetFiles(folder, "*.pth").Any())
            return new RvcModel
            {
                Name = Path.GetFileName(folder),
                FolderPath = folder
            };

        if (Directory.GetFiles(folder, "*.onnx").Any())
            return new OnnxModel
            {
                Name = Path.GetFileName(folder),
                FolderPath = folder
            };

        if (Directory.GetFiles(folder, "*.dvsmodel").Any())
            return new DvsModel
            {
                Name = Path.GetFileName(folder),
                FolderPath = folder
            };

        return new EmptyAiModel
        {
            Name = Path.GetFileName(folder),
            FolderPath = folder
        };
    }

    // ----------------------------
    // COPY FOLDER
    // ----------------------------
    private void CopyDirectory(
        string source,
        string target)
    {
        Directory.CreateDirectory(target);

        foreach (var file in Directory.GetFiles(source))
        {
            File.Copy(
                file,
                Path.Combine(target,
                    Path.GetFileName(file)),
                true);
        }

        foreach (var dir in Directory.GetDirectories(source))
        {
            CopyDirectory(
                dir,
                Path.Combine(
                    target,
                    Path.GetFileName(dir)));
        }
    }
}