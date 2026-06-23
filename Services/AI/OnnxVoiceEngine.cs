namespace DnDVoiceStudio.Services.Ai;

public class OnnxVoiceEngine : IAiVoiceEngine
{
    public string Name =>
        "ONNX";

    public bool IsLoaded =>
        false;

    public bool LoadModel(
        string modelPath)
    {
        return false;
    }

    public float[] Process(
        float[] samples)
    {
        return samples;
    }
}