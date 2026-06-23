namespace DnDVoiceStudio.Services.Ai;

public class DvsModel : IAiModel
{
    public string Name { get; set; } = "";

    public string FolderPath { get; set; } = "";

    public string ModelType => "DVS";

    public string? DvsPath { get; set; }

    public string? PthPath => null;

    public string? IndexPath => null;

    public string? OnnxPath => null;
}