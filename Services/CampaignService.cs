using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services;

public class CampaignService
{
    private readonly string _filePath =
        DataPathHelper.GetDataFile(
            "campaigns.json");

    public List<Campaign> LoadCampaigns()
    {
        try
        {
            if (!File.Exists(_filePath))
                return new();

            string json =
                File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<
                List<Campaign>>(json)
                ?? new();
        }
        catch
        {
            return new();
        }
    }

    public void SaveCampaigns(
        IEnumerable<Campaign> campaigns)
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(
                _filePath)!);

        string json =
            JsonSerializer.Serialize(
                campaigns,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            _filePath,
            json);
    }
}