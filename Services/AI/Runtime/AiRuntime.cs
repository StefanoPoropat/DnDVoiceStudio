namespace DnDVoiceStudio.Services.AI.Runtime;

public sealed class AiRuntime
{
    private IInferenceBackend? _backend;

    public bool IsInitialized { get; private set; }

    public void SetBackend(IInferenceBackend backend)
    {
        _backend = backend;
    }

    public bool Initialize()
    {
        if (_backend == null)
            return false;

        IsInitialized = _backend.Initialize();
        return IsInitialized;
    }

    public void Shutdown()
    {
        _backend?.Shutdown();
        IsInitialized = false;
    }

    public float[] Run(float[] samples)
    {
        if (!IsInitialized || _backend == null)
        {
            System.Diagnostics.Debug.WriteLine("AI RUNTIME NOT INITIALIZED");
            return samples;
        }

        return _backend.Process(samples);
    }
}