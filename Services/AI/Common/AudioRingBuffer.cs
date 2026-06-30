namespace DnDVoiceStudio.Services.AI.Common;

public sealed class AudioRingBuffer
{
    private readonly float[] _buffer;

    private int _writeIndex;
    private int _readIndex;
    private int _count;

    public AudioRingBuffer(int capacity)
    {
        Capacity = capacity;
        _buffer = new float[capacity];
    }

    public int Capacity { get; }

    public int Count => _count;

    public void Clear()
    {
        Array.Clear(_buffer);

        _writeIndex = 0;
        _readIndex = 0;
        _count = 0;
    }

    public void Write(ReadOnlySpan<float> samples)
    {
        foreach (float sample in samples)
        {
            _buffer[_writeIndex] = sample;

            _writeIndex =
                (_writeIndex + 1) % Capacity;

            if (_count < Capacity)
            {
                _count++;
            }
            else
            {
                _readIndex =
                    (_readIndex + 1) % Capacity;
            }
        }
    }

    public float[] Peek(int sampleCount)
    {
        sampleCount =
            Math.Min(sampleCount, _count);

        float[] result =
            new float[sampleCount];

        int index = _readIndex;

        for (int i = 0; i < sampleCount; i++)
        {
            result[i] = _buffer[index];

            index =
                (index + 1) % Capacity;
        }

        return result;
    }

    public void Skip(int sampleCount)
    {
        sampleCount =
            Math.Min(sampleCount, _count);

        _readIndex =
            (_readIndex + sampleCount) % Capacity;

        _count -= sampleCount;
    }

    public float[] Read(int sampleCount)
    {
        float[] result =
            Peek(sampleCount);

        Skip(result.Length);

        return result;
    }
}