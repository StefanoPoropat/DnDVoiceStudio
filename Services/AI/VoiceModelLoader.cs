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

        foreach (string folderPath in
                 Directory.GetDirectories(rootFolder))
        {
            string name =
                Path.GetFileName(folderPath);

            string[] pthFiles =
                Directory.GetFiles(folderPath, "*.pth");

            string[] onnxFiles =
                Directory.GetFiles(folderPath, "*.onnx");

            string[] dvsFiles =
                Directory.GetFiles(folderPath, "*.dvsmodel");

            string[] indexFiles =
                Directory.GetFiles(folderPath, "*.index");

            // RVC MODEL
            if (pthFiles.Length > 0)
            {
                models.Add(new RvcModel
                {
                    Name = name,
                    FolderPath = folderPath,
                    PthPath = pthFiles.FirstOrDefault(),
                    IndexPath = indexFiles.FirstOrDefault()
                });

                continue;
            }

            // ONNX MODEL
            if (onnxFiles.Length > 0)
            {
                models.Add(new OnnxModel
                {
                    Name = name,
                    FolderPath = folderPath,
                    OnnxPath = onnxFiles.FirstOrDefault()
                });

                continue;
            }

            // DVS MODEL
            if (dvsFiles.Length > 0)
            {
                models.Add(new DvsModel
                {
                    Name = name,
                    FolderPath = folderPath,
                    DvsPath = dvsFiles.FirstOrDefault()
                });

                continue;
            }

            // EMPTY MODEL (CONFIG ONLY)
            models.Add(new EmptyAiModel
            {
                Name = name,
                FolderPath = folderPath
            });
        }

        return models;
    }

    private string DetectType(string folder)
    {
        if (Directory.GetFiles(folder, "*.pth").Any())
            return "RVC";

        if (Directory.GetFiles(folder, "*.onnx").Any())
            return "ONNX";

        if (Directory.GetFiles(folder, "*.dvsmodel").Any())
            return "DVS";

        return "EMPTY";
    }
}