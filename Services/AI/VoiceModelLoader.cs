using System.IO;

namespace DnDVoiceStudio.Services.Ai;

public class VoiceModelLoader
{
    public List<IAiModel> LoadModels(
        string rootFolder)
    {
        List<IAiModel> models = new();

        if (!Directory.Exists(rootFolder))
            return models;

        foreach (string folder in
                 Directory.GetDirectories(rootFolder))
        {
            string name =
                Path.GetFileName(folder);

            string[] pthFiles =
                Directory.GetFiles(
                    folder,
                    "*.pth",
                    SearchOption.TopDirectoryOnly);

            string[] indexFiles =
                Directory.GetFiles(
                    folder,
                    "*.index",
                    SearchOption.TopDirectoryOnly);

            string[] onnxFiles =
                Directory.GetFiles(
                    folder,
                    "*.onnx",
                    SearchOption.TopDirectoryOnly);

            string[] dvsFiles =
                Directory.GetFiles(
                    folder,
                    "*.dvsmodel",
                    SearchOption.TopDirectoryOnly);

            if (pthFiles.Length > 0)
            {
                models.Add(
                    new RvcModel
                    {
                        Name = name,
                        FolderPath = folder,
                        PthPath = pthFiles[0],
                        IndexPath =
                            indexFiles.FirstOrDefault()
                    });

                continue;
            }

            if (onnxFiles.Length > 0)
            {
                models.Add(
                    new OnnxModel
                    {
                        Name = name,
                        FolderPath = folder,
                        OnnxPath = onnxFiles[0]
                    });

                continue;
            }

            if (dvsFiles.Length > 0)
            {
                models.Add(
                    new DvsModel
                    {
                        Name = name,
                        FolderPath = folder,
                        DvsPath = dvsFiles[0]
                    });

                continue;
            }
        }

        return models;
    }
}