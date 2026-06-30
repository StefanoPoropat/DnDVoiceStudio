namespace DnDVoiceStudio.Services.AI.Common;

public sealed class InferenceStatistics
{
    public long FramesProcessed { get; internal set; }

    public long SamplesProcessed { get; internal set; }

    public long FramesDropped { get; internal set; }

    public TimeSpan TotalInferenceTime { get; internal set; }

    public double AverageInferenceMilliseconds =>
        FramesProcessed == 0
            ? 0
            : TotalInferenceTime.TotalMilliseconds /
              FramesProcessed;

    public void Reset()
    {
        FramesProcessed = 0;
        SamplesProcessed = 0;
        FramesDropped = 0;
        TotalInferenceTime = TimeSpan.Zero;
    }
}