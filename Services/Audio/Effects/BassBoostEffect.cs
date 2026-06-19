namespace DnDVoiceStudio.Services.Audio.Effects;

public class BassBoostEffect
{
    private float _amount;

    public void SetAmount(
        float amount)
    {
        _amount = amount;
    }

    public float[] Process(
        float[] samples)
    {
        if (_amount == 0)
            return samples;

        float gain =
            1f + (_amount / 20f);

        float[] output =
            new float[samples.Length];

        float previous = 0;

        for (int i = 0; i < samples.Length; i++)
        {
            previous =
                (previous * 0.95f) +
                (samples[i] * 0.05f);

            output[i] =
                samples[i] +
                previous * gain;
        }

        return output;
    }
}