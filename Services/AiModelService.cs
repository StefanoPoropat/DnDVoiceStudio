using DnDVoiceStudio.Models;
using System.IO;

namespace DnDVoiceStudio.Services;

public class AiModelService
{
    public List<AiVoiceModel> LoadModels()
    {
        string modelsFolder =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Models");

        if (!Directory.Exists(modelsFolder))
            Directory.CreateDirectory(modelsFolder);

        return Directory
            .GetFiles(modelsFolder, "*.pth",
                SearchOption.AllDirectories)
            .Select(file =>
                new AiVoiceModel
                {
                    Name =
                        Path.GetFileNameWithoutExtension(file),

                    ModelPath = file
                })
            .ToList();
    }
}