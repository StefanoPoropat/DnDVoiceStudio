namespace DnDVoiceStudio.Models;

public class SoundMetadata
{
    public string Name { get; set; } = "";

    public bool IsFavorite { get; set; }

    public string Hotkey { get; set; } = "";
    public float Volume { get; set; }
}