using System.IO;

namespace DnDVoiceStudio.Services.Ai;

public class RvcAiVoiceEngine : IAiVoiceEngine
{
    public string Name =>
        "DnD Voice Studio AI";

    public bool IsLoaded
    {
        get;
        private set;
    }

    private string? _loadedModel;

    public bool LoadModel(
        string modelPath)
    {
        if (!File.Exists(modelPath))
            return false;

        _loadedModel = modelPath;

        IsLoaded = true;

        return true;
    }

    public float[] Process(
        float[] samples)
    {
        if (!IsLoaded)
            return samples;

        // AI processing will go here later

        return samples;
    }
}