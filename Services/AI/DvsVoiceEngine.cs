namespace DnDVoiceStudio.Services.Ai;

public class DvsVoiceEngine
    : IAiVoiceEngine
{
    public string Name =>
        "DVS";

    public bool IsLoaded =>
        false;

    public bool LoadModel(
        string modelFolder)
    {
        throw new NotImplementedException();
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