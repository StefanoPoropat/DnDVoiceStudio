namespace DnDVoiceStudio.Services.AI.Common;

public sealed class FrameExtractor
{
    private readonly AudioRingBuffer _buffer;

    public FrameExtractor(
        int sampleRate,
        int frameSize,
        int hopSize)
    {
        SampleRate = sampleRate;

        FrameSize = frameSize;

        HopSize = hopSize;

        _buffer =
            new AudioRingBuffer(
                frameSize * 4);
    }

    public int SampleRate { get; }

    public int FrameSize { get; }

    public int HopSize { get; }

    public bool HasFrame =>
        _buffer.Count >= FrameSize;

    public void Push(float[] samples)
    {
        _buffer.Write(samples);
    }

    public AudioFrame GetFrame()
    {
        float[] frame =
            _buffer.Peek(FrameSize);

        _buffer.Skip(HopSize);

        return new AudioFrame(
            frame,
            SampleRate);
    }

    public void Clear()
    {
        _buffer.Clear();
    }
}