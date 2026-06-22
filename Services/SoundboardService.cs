using DnDVoiceStudio.Models;
using System.IO;
using System.Text.Json;

namespace DnDVoiceStudio.Services;

public class SoundboardService
{
    public List<SoundboardItem> LoadSounds()
    {

        var sounds = new List<SoundboardItem>();

        var metadata =
            LoadMetadata();
        string root =
            DataPathHelper.SoundsFolder;

        System.Diagnostics.Debug.WriteLine(
            $"Looking for sounds in: {root}");

        if (!Directory.Exists(root))
        {
            System.Diagnostics.Debug.WriteLine(
                "Sound folder not found.");

            return sounds;
        }

        foreach (string file in Directory.GetFiles(
                     root,
                     "*.wav",
                     SearchOption.AllDirectories))
        {
            System.Diagnostics.Debug.WriteLine(
                $"Found: {file}");

            var meta =
    metadata.FirstOrDefault(
        x => x.Name ==
             Path.GetFileNameWithoutExtension(
                 file));

            sounds.Add(
                new SoundboardItem
                {
                    Name =
                        Path.GetFileNameWithoutExtension(
                            file),

                    FilePath = file,

                    Category =
                        new DirectoryInfo(
                            Path.GetDirectoryName(
                                file)!)
                        .Name,

                    IsFavorite =
                        meta?.IsFavorite ?? false,

                    Hotkey =
                        meta?.Hotkey ?? ""
                });

        }

        return sounds;
    }

    private readonly string _metadataPath =
        DataPathHelper.GetDataFile(
            "soundboard.json");

    private List<SoundMetadata>
    LoadMetadata()
    {
        try
        {
            if (!File.Exists(
                _metadataPath))
            {
                return new();
            }

            string json =
                File.ReadAllText(
                    _metadataPath);

            return JsonSerializer
                .Deserialize<
                    List<SoundMetadata>>(
                        json)
                   ?? new();
        }
        catch
        {
            return new();
        }
    }

    public void SaveMetadata(
    IEnumerable<SoundboardItem> sounds)
    {
        Directory.CreateDirectory(
            Path.GetDirectoryName(
                _metadataPath)!);

        var metadata =
            sounds.Select(
                x => new SoundMetadata
                {
                    Name = x.Name,
                    IsFavorite =
                        x.IsFavorite,
                    Hotkey =
                        x.Hotkey
                });

        string json =
            JsonSerializer.Serialize(
                metadata,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            _metadataPath,
            json);
    }

}