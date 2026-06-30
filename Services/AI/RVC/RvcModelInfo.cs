namespace DnDVoiceStudio.Services.Ai.RVC;

public sealed class RvcModelInfo
{
    public string Name { get; set; } = "";

    public string FolderPath { get; set; } = "";

    public string PthPath { get; set; } = "";

    public string? IndexPath { get; set; }

    public string ConfigPath { get; set; } = "";

    public long ModelSizeBytes { get; set; }

    public bool HasIndex =>
        !string.IsNullOrWhiteSpace(IndexPath);

    public DateTime LoadedTime { get; set; }

    public bool IsValid { get; set; }
}