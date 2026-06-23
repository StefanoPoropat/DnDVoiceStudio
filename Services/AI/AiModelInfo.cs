using DnDVoiceStudio.Models;

namespace DnDVoiceStudio.Services.Ai;

public class AiModelInfo
{
    public string Name { get; set; } = "";

    public string ModelPath { get; set; } = "";

    public string ModelType { get; set; } = "";

    public string FolderPath { get; set; } = "";

    public string? PortraitPath { get; set; }

    public string? IndexPath { get; set; }

    public string? OnnxPath { get; set; }
    public VoiceModelConfig Config { get; set; } = new();
}