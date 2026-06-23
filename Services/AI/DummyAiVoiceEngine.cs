namespace DnDVoiceStudio.Services.Ai;

public class DummyAiVoiceEngine
    : IAiVoiceEngine
{
    public string Name =>
        "Dummy";

    public bool IsLoaded =>
        false;

    public bool LoadModel(
        string modelFolder)
    {
        return false;
    }

    public float[] Process(
        float[] samples)
    {
        return samples;
    }

    public void Unload()
    {
    }
}