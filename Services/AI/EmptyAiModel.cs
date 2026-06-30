using DnDVoiceStudio.Services.AI.Interfaces;

namespace DnDVoiceStudio.Services.Ai;

public class EmptyAiModel : IAiModel
{
    public string Name { get; set; } = "";
    public string FolderPath { get; set; } = "";
    public string ModelType => "EMPTY";

    public string? PthPath => null;
    public string? IndexPath => null;
    public string? OnnxPath => null;
    public string? DvsPath => null;
}