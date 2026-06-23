namespace DnDVoiceStudio.Services.Ai;

public class OnnxVoiceEngine
    : IAiVoiceEngine
{
    public string Name =>
        "ONNX";

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