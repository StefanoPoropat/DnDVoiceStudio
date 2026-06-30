namespace DnDVoiceStudio.Services.AI.Common;

public sealed class AudioFrame
{
    public AudioFrame(float[] samples, int sampleRate)
    {
        Samples = samples;
        SampleRate = sampleRate;
    }

    public float[] Samples { get; }

    public int SampleRate { get; }

    public int Length => Samples.Length;

    public TimeSpan Duration =>
        TimeSpan.FromSeconds(
            (double)Length / SampleRate);
}