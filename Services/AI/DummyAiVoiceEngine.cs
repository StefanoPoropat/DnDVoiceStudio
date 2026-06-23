namespace DnDVoiceStudio.Services.Ai;

public class DummyAiVoiceEngine : IAiVoiceEngine
{
    public string Name =>
        "Dummy";

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
/* FOR POSSIBLE FUTURE USE:
 
namespace DnDVoiceStudio.Services.Ai;

public class OnnxVoiceEngine : IAiVoiceEngine
{
    private string _name = "ONNX";

    public string Name => _name;

    public bool IsLoaded
    {
        get;
        private set;
    }

    public bool LoadModel(
        string modelPath)
    {
        _name =
            Path.GetFileNameWithoutExtension(
                modelPath);

        IsLoaded = true;

        return true;
    }

    public float[] Process(
        float[] samples)
    {
        return samples;
    }
} 

 */