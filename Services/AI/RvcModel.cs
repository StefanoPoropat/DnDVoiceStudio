namespace DnDVoiceStudio.Services.Ai;

public class RvcModel : IAiModel
{
    public string Name { get; set; } = "";

    public string FolderPath { get; set; } = "";

    public string ModelType => "RVC";

    public string? PthPath { get; set; }

    public string? IndexPath { get; set; }

    public string? OnnxPath => null;

    public string? DvsPath => null;
}