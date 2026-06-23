using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services.Ai;

public class ModelDiscoveryService
{
    public List<ModelInfo> DiscoverModels(
        string rootFolder)
    {
        var result =
            new List<ModelInfo>();

        if (!Directory.Exists(
            rootFolder))
        {
            return result;
        }

        foreach (var folder in
                 Directory.GetDirectories(
                     rootFolder))
        {
            string configPath =
                Path.Combine(
                    folder,
                    "config.json");

            if (!File.Exists(
                configPath))
            {
                continue;
            }

            try
            {
                string json =
                    File.ReadAllText(
                        configPath);

                var model =
                    JsonSerializer.Deserialize<ModelInfo>(
                        json);

                if (model == null)
                    continue;

                model.FolderPath =
                    folder;

                result.Add(
                    model);
            }
            catch
            {
            }
        }

        return result;
    }
}