namespace DnDVoiceStudio.Services.Ai;

public interface IAiVoiceEngine
{
    string Name { get; }

    bool IsLoaded { get; }

    bool LoadModel(string modelFolder);

    float[] Process(float[] samples);

    void Unload();
}