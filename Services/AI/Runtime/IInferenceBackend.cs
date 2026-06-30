namespace DnDVoiceStudio.Services.AI.Runtime;

public interface IInferenceBackend
{
    bool Initialize();

    void Shutdown();

    float[] Process(float[] input);
}