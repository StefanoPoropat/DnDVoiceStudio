using DnDVoiceStudio.Services.Ai.Common;
using DnDVoiceStudio.Services.AI.Common;

namespace DnDVoiceStudio.Services.AI.Interfaces;

public interface IAiVoiceEngine
{
    AiEngineState State { get; }

    bool IsLoaded => State == AiEngineState.Loaded;

    bool LoadModel(string modelPath);

    void Unload();

    float[] Process(float[] samples);

    LatencyTracker Latency { get; }
    InferenceStatistics Statistics { get; }
}