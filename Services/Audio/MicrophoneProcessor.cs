using NAudio.Wave;

namespace DnDVoiceStudio.Services.Audio;

public class MicrophoneProcessor : IAudioProcessor
{
    private WaveInEvent? _waveIn;

    public void Start()
    {
        if (_waveIn != null)
            return;

        _waveIn = new WaveInEvent
        {
            WaveFormat = new WaveFormat(44100, 1)
        };

        _waveIn.DataAvailable += OnDataAvailable;

        _waveIn.StartRecording();
    }

    public void Stop()
    {
        _waveIn?.StopRecording();
        _waveIn?.Dispose();

        _waveIn = null;
    }

    private void OnDataAvailable(
        object? sender,
        WaveInEventArgs e)
    {
        // audio processing goes here later
    }
}