using DnDVoiceStudio.Services.AI.Interfaces;

namespace DnDVoiceStudio.Services.AI.ONNX;

public class OnnxModel : IAiModel
{
    public string Name { get; set; } = "";

    public string FolderPath { get; set; } = "";

    public string ModelType => "ONNX";

    public string? OnnxPath { get; set; }

    public string? PthPath => null;

    public string? IndexPath => null;

    public string? DvsPath => null;
}