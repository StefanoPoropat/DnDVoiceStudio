namespace DnDVoiceStudio.Services.Ai;

public interface IAiVoiceEngine
{
    string Name
    {
        get;
    }

    bool IsLoaded
    {
        get;
    }

    bool LoadModel(
        string modelPath);

    float[] Process(
        float[] samples);
}