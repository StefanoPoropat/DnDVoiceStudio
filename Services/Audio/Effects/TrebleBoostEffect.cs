namespace DnDVoiceStudio.Services.Audio.Effects;

public class TrebleBoostEffect
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
            _amount / 20f;

        float[] output =
            new float[samples.Length];

        float previous = 0;

        for (int i = 0; i < samples.Length; i++)
        {
            float high =
                samples[i] - previous;

            output[i] =
                samples[i] +
                high * gain;

            previous =
                samples[i];
        }

        return output;
    }
}