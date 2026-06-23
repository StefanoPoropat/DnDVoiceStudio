using System.IO;

namespace DnDVoiceStudio.Services.Ai;

public class RvcVoiceEngine
    : AiVoiceEngineBase
{
    public override string Name =>
        "RVC";

    public override bool LoadModel(
        string modelFolder)
    {
        string pth =
            Directory.GetFiles(
                modelFolder,
                "*.pth")
            .FirstOrDefault()
            ?? string.Empty;

        if (!File.Exists(pth))
            return false;

        string index =
            Directory.GetFiles(
                modelFolder,
                "*.index")
            .FirstOrDefault()
            ?? string.Empty;

        LoadedModel =
            new RvcModel
            {
                Name =
                    Path.GetFileName(
                        modelFolder),

                FolderPath =
                    modelFolder,

                PthPath = pth,

                IndexPath = index
            };

        return true;
    }
}