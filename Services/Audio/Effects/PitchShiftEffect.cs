using SoundTouch;

namespace DnDVoiceStudio.Services.Audio.Effects;

public class PitchShiftEffect : IAudioEffect
{
    private readonly SoundTouchProcessor _processor;

    public PitchShiftEffect()
    {
        _processor = new SoundTouchProcessor();

        _processor.SampleRate = 16000;
        _processor.Channels = 1;
    }

    public void SetPitch(float semitones)
    {
        _processor.PitchSemiTones = semitones;
    }

    public float[] Process(float[] samples)
    {
        _processor.PutSamples(
            samples,
            samples.Length);

        float[] output =
            new float[samples.Length * 2];

        int received =
            _processor.ReceiveSamples(
                output,
                output.Length);

        if (received <= 0)
            return Array.Empty<float>();

        return output
            .Take(received)
            .ToArray();
    }
}