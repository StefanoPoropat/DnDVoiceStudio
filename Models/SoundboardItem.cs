namespace DnDVoiceStudio.Models;

public class SoundboardItem
{
    public string Name { get; set; } = "";

    public string FilePath { get; set; } = "";

    public string Category { get; set; } = "General";
    public bool IsFavorite { get; set; }

    public string Hotkey { get; set; } = "";
    public bool Loop { get; set; }
}