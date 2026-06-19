using NAudio.Wave;

namespace DnDVoiceStudio.Services;

public class SoundEffectPlayer
{
    private readonly List<WaveOutEvent> _activeOutputs =
        new();

    private readonly List<AudioFileReader> _activeReaders =
        new();

    public void Play(string filePath)
    {
        var reader =
            new AudioFileReader(filePath);

        var output =
            new WaveOutEvent();

        output.Init(reader);

        _activeOutputs.Add(output);
        _activeReaders.Add(reader);

        output.PlaybackStopped += (_, _) =>
        {
            output.Dispose();
            reader.Dispose();

            _activeOutputs.Remove(output);
            _activeReaders.Remove(reader);
        };

        output.Play();
    }

    public void StopAll()
    {
        foreach (var output in _activeOutputs.ToList())
        {
            output.Stop();
        }

        _activeOutputs.Clear();

        foreach (var reader in _activeReaders.ToList())
        {
            reader.Dispose();
        }

        _activeReaders.Clear();
    }
}