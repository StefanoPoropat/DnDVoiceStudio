using DnDVoiceStudio.Models;
using System.IO;

namespace DnDVoiceStudio.Services;

public class SoundboardService
{
    public List<SoundboardItem> LoadSounds()
    {
        var sounds = new List<SoundboardItem>();

        string root =
            Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Sounds");

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

            sounds.Add(
                new SoundboardItem
                {
                    Name =
                        Path.GetFileNameWithoutExtension(file),

                    FilePath = file,

                    Category =
                        new DirectoryInfo(
                            Path.GetDirectoryName(file)!)
                        .Name
                });
        }

        return sounds;
    }
}