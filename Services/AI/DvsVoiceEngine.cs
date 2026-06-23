using System.IO;

namespace DnDVoiceStudio.Services.Ai;

public class DvsVoiceEngine
    : AiVoiceEngineBase
{
    public override string Name =>
        "DVS";

    public override bool LoadModel(
        string modelFolder)
    {
        string dvs =
            Directory.GetFiles(
                modelFolder,
                "*.dvsmodel")
            .FirstOrDefault()
            ?? string.Empty;

        if (!File.Exists(dvs))
            return false;

        LoadedModel =
            new DvsModel
            {
                Name =
                    Path.GetFileName(
                        modelFolder),

                FolderPath =
                    modelFolder,

                DvsPath = dvs
            };

        return true;
    }
}