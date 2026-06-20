namespace DnDVoiceStudio.Models;

public class SoundboardItem
{
    public string Name { get; set; } = "";

    public string FilePath { get; set; } = "";

    public string Category { get; set; } = "";

    public bool IsFavorite { get; set; }
}