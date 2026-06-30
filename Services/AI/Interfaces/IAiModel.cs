namespace DnDVoiceStudio.Services.AI.Interfaces;

public interface IAiModel
{
    string Name { get; }

    string FolderPath { get; }

    string ModelType { get; }

    string? PthPath { get; }

    string? IndexPath { get; }

    string? OnnxPath { get; }

    string? DvsPath { get; }
}