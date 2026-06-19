namespace DnDVoiceStudio.Services.Audio.Effects;

public class DemonEffect
{
    private float _amount = 0;

    public void SetAmount(
        float amount)
    {
        _amount = amount;
    }

    public float[] Process(
    float[] samples)
    {
        if (_amount <= 0)
            return samples;

        float mix =
            _amount / 100f;

        float[] result =
            new float[samples.Length];

        int delaySamples =
            (int)(20 * mix);

        for (int i = 0;
             i < samples.Length;
             i++)
        {
            float clean =
                samples[i];

            float distorted =
                (float)Math.Tanh(
                    clean *
                    (1f + mix * 10f));

            float delayed = 0f;

            if (i >= delaySamples)
            {
                delayed =
                    samples[
                        i - delaySamples]
                    * 0.35f
                    * mix;
            }

            float demon =
                distorted +
                delayed;

            result[i] =
                Math.Clamp(
                    clean * (1f - mix)
                    + demon * mix,
                    -1f,
                    1f);
        }

        return result;
    }

}