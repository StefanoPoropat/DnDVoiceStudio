namespace DnDVoiceStudio.Services.AI.Streaming;

public sealed class OverlapAddBuffer
{
    private readonly float[] _buffer;
    private readonly float[] _temp;

    public OverlapAddBuffer(int capacity)
    {
        _buffer = new float[capacity];
        _temp = new float[capacity];
    }

    public void Add(ReadOnlySpan<float> samples)
    {
        int count = Math.Min(samples.Length, _buffer.Length);

        for (int i = 0; i < count; i++)
        {
            _buffer[i] += samples[i];
        }
    }

    public float[] Consume(int count)
    {
        count = Math.Min(count, _buffer.Length);

        Array.Copy(_buffer, _temp, _buffer.Length);

        Array.Copy(
            _buffer,
            count,
            _buffer,
            0,
            _buffer.Length - count);

        Array.Clear(
            _buffer,
            _buffer.Length - count,
            count);

        float[] output = new float[count];
        Array.Copy(_temp, output, count);

        return output;
    }

    public void Clear()
    {
        Array.Clear(_buffer);
        Array.Clear(_temp);
    }
}