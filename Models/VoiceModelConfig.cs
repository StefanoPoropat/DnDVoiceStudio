namespace DnDVoiceStudio.Models;

public class VoiceModelConfig
{
    public string DisplayName { get; set; } = "";
    public string Author { get; set; } = "";
    public string Description { get; set; } = "";
    public string Version { get; set; } = "1.0";

    public string ModelType { get; set; } = "EMPTY";

    public string Race { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Language { get; set; } = "";
    public string Tags { get; set; } = "";
}