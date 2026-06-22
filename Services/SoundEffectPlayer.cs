using NAudio.Wave;

namespace DnDVoiceStudio.Services;

public class SoundEffectPlayer
{
    private readonly Dictionary<string, (WaveOutEvent output, AudioFileReader reader)> _active
        = new();

    private readonly object _lock = new();

    public void Play(string filePath, float volume, bool loop)
    {
        lock (_lock)
        {
            if (_active.TryGetValue(filePath, out var existing))
            {
                existing.reader.Position = 0;
                existing.output.Stop();
                existing.output.Play();
                return;
            }

            var reader = new AudioFileReader(filePath)
            {
                Volume = volume
            };

            IWaveProvider provider = loop ? new LoopStream(reader) : reader;

            var output = new WaveOutEvent();
            output.Init(provider);

            _active[filePath] = (output, reader);

            output.PlaybackStopped += (_, _) =>
            {
                lock (_lock)
                {
                    if (_active.TryGetValue(filePath, out var current) &&
                        current.output == output)
                    {
                        _active.Remove(filePath);
                    }
                }

                output.Dispose();
                reader.Dispose();
            };

            output.Play();
        }
    }

    public void Stop(string filePath)
    {
        lock (_lock)
        {
            if (_active.TryGetValue(filePath, out var entry))
            {
                entry.output.Stop();
            }
        }
    }

    public void StopAll()
    {
        lock (_lock)
        {
            foreach (var entry in _active.Values.ToList())
            {
                entry.output.Stop();
            }

            _active.Clear();
        }
    }
}