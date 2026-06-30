using DnDVoiceStudio.Services.Ai.Common;
using DnDVoiceStudio.Services.AI.Common;
using DnDVoiceStudio.Services.AI.Interfaces;
using DnDVoiceStudio.Services.AI.Runtime;
using System.Diagnostics;

namespace DnDVoiceStudio.Services.AI.Engines;

public abstract class AiVoiceEngineBase : IAiVoiceEngine
{
    public AiEngineState State { get; protected set; }
        = AiEngineState.Unloaded;

    public bool IsLoaded =>
        State == AiEngineState.Loaded;

    public abstract bool LoadModel(string modelPath);

    public abstract void Unload();

    public abstract float[] Process(float[] samples);

    protected void BeginLoading()
    {
        State = AiEngineState.Loading;
    }

    protected void FinishLoading()
    {
        State = AiEngineState.Loaded;
    }

    protected void Fail()
    {
        State = AiEngineState.Error;
    }

    protected void Reset()
    {
        State = AiEngineState.Unloaded;
    }
    public InferenceStatistics Statistics { get; } =
    new();
    protected Stopwatch BeginInference()
    {
        return Stopwatch.StartNew();
    }

    protected void EndInference(
        Stopwatch stopwatch,
        int sampleCount)
    {
        stopwatch.Stop();

        Statistics.FramesProcessed++;

        Statistics.SamplesProcessed +=
            sampleCount;

        Statistics.TotalInferenceTime +=
            stopwatch.Elapsed;
    }
    protected AiRuntime Runtime { get; } =
    new();
    protected void UseBackend(IInferenceBackend backend)
    {
        Runtime.SetBackend(backend);
    }
    public LatencyTracker Latency { get; } =
    new();
}