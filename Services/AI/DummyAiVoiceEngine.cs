namespace DnDVoiceStudio.Services.Ai;

public class DummyAiVoiceEngine : IAiVoiceEngine
{
    public string Name =>
        "Dummy AI";

    public bool IsLoaded
    {
        get;
        private set;
    }

    public bool LoadModel(
        string modelPath)
    {
        IsLoaded = true;

        return true;
    }

    public float[] Process(
        float[] samples)
    {
        return samples;
    }
}