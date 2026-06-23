namespace DnDVoiceStudio.Services.Audio.Effects;

public class TrebleBoostEffect
{
    private float _amount;

    public void SetAmount(float amount)
    {
        _amount = amount;
    }

    public float[] Process(float[] samples)
    {
        if (Math.Abs(_amount) < 0.01f)
            return samples;

        float gain =
            (float)Math.Pow(
                10,
                _amount / 40f);

        float[] output =
            new float[samples.Length];

        float previous = 0;

        for (int i = 0; i < samples.Length; i++)
        {
            float high =
                samples[i] - previous;

            output[i] =
                samples[i] +
                high * (gain - 1f);

            previous = samples[i];
        }

        return output;
    }
}