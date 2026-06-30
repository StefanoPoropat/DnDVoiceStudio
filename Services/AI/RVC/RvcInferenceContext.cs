using DnDVoiceStudio.Services.Ai.RVC;
using DnDVoiceStudio.Services.AI.Streaming;

namespace DnDVoiceStudio.Services.AI.RVC;

public sealed class RvcInferenceContext
{
    public RvcModelInfo? Model { get; set; }

    public AudioPreprocessor Preprocessor { get; } =
        new();

    public AudioPostprocessor Postprocessor { get; } =
        new();

    public StreamingAudioPipeline Pipeline { get; } =
        new(
            sampleRate: 16000,
            frameSize: 3200,
            hopSize: 1600);

    public bool IsInitialized =>
        Model != null;
}