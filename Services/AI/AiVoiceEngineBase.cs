namespace DnDVoiceStudio.Services.Ai;

public abstract class AiVoiceEngineBase
    : IAiVoiceEngine
{
    protected IAiModel? LoadedModel;

    public bool IsLoaded =>
        LoadedModel != null;

    public abstract string Name
    { get; }

    public abstract bool LoadModel(
        string modelFolder);

    public virtual float[] Process(
        float[] samples)
    {
        return samples;
    }

    public virtual void Unload()
    {
        LoadedModel = null;
    }
}