namespace DnDVoiceStudio.Models;

public class VoiceModelConfig
{
    public string Name { get; set; } = "";

    public int SampleRate { get; set; } = 16000;

    public int ChunkSize { get; set; } = 3200;

    public string InputName { get; set; } = "input";

    public string OutputName { get; set; } = "output";

    public int SpeakerId { get; set; }
}