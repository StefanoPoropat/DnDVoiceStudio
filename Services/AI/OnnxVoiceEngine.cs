using System.IO;

namespace DnDVoiceStudio.Services.Ai;

public class OnnxVoiceEngine : IAiVoiceEngine
{
    private bool _loaded;

    private string _name = "ONNX";

    public string Name =>
        _name;

    public bool IsLoaded =>
        _loaded;

    public bool LoadModel(
        string modelPath)
    {
        _name =
            Path.GetFileName(
                Path.GetDirectoryName(
                    modelPath)!);

        _loaded = true;

        return true;
    }

    public float[] Process(
        float[] samples)
    {
        return samples;
    }
}