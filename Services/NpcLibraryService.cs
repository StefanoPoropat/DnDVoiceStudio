using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services;

public class NpcLibraryService
{
    private readonly string _filePath =
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Data",
            "npcs.json");

    public List<NpcProfile> LoadNpcs()
    {
        try
        {
            if (!File.Exists(_filePath))
                return new();

            string json =
                File.ReadAllText(_filePath);

            return JsonSerializer.Deserialize<
                List<NpcProfile>>(json)
                ?? new();
        }
        catch
        {
            return new();
        }
    }

    public void SaveNpcs(
        IEnumerable<NpcProfile> npcs)
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(
                _filePath)!);

        string json =
            JsonSerializer.Serialize(
                npcs,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            _filePath,
            json);
    }
}