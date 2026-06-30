using DnDVoiceStudio.Services.AI.Common;

namespace DnDVoiceStudio.Services.AI.Streaming;

public sealed class StreamingAudioPipeline
{
    private readonly FrameExtractor _extractor;
    private readonly OverlapAddBuffer _outputBuffer;
    private readonly float[] _window;

    public StreamingAudioPipeline(
        int sampleRate,
        int frameSize,
        int hopSize)
    {
        _extractor =
            new FrameExtractor(
                sampleRate,
                frameSize,
                hopSize);

        _outputBuffer =
            new OverlapAddBuffer(
                frameSize * 4);

        _window =
            WindowFunctions.Hann(frameSize);

        FrameSize = frameSize;
        HopSize = hopSize;
    }

    public int FrameSize { get; }

    public int HopSize { get; }

    public void Push(float[] samples)
    {
        _extractor.Push(samples);
    }

    public bool HasFrame =>
        _extractor.HasFrame;

    public AudioFrame GetFrame()
    {
        return _extractor.GetFrame();
    }

    public void AddProcessedFrame(float[] samples)
    {
        // apply window BEFORE overlap-add
        WindowFunctions.Apply(samples, _window);

        _outputBuffer.Add(samples);
    }

    public float[] GetOutput()
    {
        return _outputBuffer.Consume(HopSize);
    }

    public void Clear()
    {
        _extractor.Clear();
        _outputBuffer.Clear();
    }
}