using DnDVoiceStudio.Services.Ai.RVC;
using System.IO;

namespace DnDVoiceStudio.Services.AI.RVC;

public sealed class RvcModelLoader
{
    public RvcModelInfo? Load(string folder)
    {
        if (!RvcModelValidator.Validate(folder))
            return null;

        string pth =
            Path.Combine(folder, "model.pth");

        string config =
            Path.Combine(folder, "config.json");

        string index =
            Path.Combine(folder, "model.index");

        return new RvcModelInfo
        {
            Name = Path.GetFileName(folder),

            FolderPath = folder,

            PthPath = pth,

            ConfigPath = config,

            IndexPath =
                File.Exists(index)
                    ? index
                    : null,

            ModelSizeBytes =
                new FileInfo(pth).Length,

            LoadedTime =
                DateTime.Now,

            IsValid = true
        };
    }
}