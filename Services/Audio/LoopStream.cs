using NAudio.Wave;

public class LoopStream : WaveStream
{
    private readonly WaveStream _source;

    public LoopStream(
        WaveStream source)
    {
        _source = source;
    }

    public override WaveFormat WaveFormat =>
        _source.WaveFormat;

    public override long Length =>
        _source.Length;

    public override long Position
    {
        get => _source.Position;
        set => _source.Position = value;
    }

    public override int Read(
        byte[] buffer,
        int offset,
        int count)
    {
        int totalRead = 0;

        while (totalRead < count)
        {
            int read =
                _source.Read(
                    buffer,
                    offset + totalRead,
                    count - totalRead);

            if (read == 0)
            {
                _source.Position = 0;
            }
            else
            {
                totalRead += read;
            }
        }

        return totalRead;
    }
}