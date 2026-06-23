public class DragonEffect
{
    private float _amount;

    public void SetAmount(float amount)
    {
        _amount = amount;
    }

    public float[] Process(float[] samples)
    {
        if (_amount <= 0)
            return samples;

        float mix =
            _amount / 100f;

        float[] result =
            new float[samples.Length];

        int delaySamples =
            (int)(120 * mix);

        for (int i = 0; i < samples.Length; i++)
        {
            float clean =
                samples[i];

            float growl =
                (float)Math.Tanh(
                    clean *
                    (1f + mix * 6f));

            if (i >= delaySamples)
            {
                growl +=
                    samples[i - delaySamples]
                    * 0.45f
                    * mix;
            }

            result[i] =
                Math.Clamp(
                    clean * (1f - mix)
                    + growl * mix,
                    -1f,
                    1f);
        }

        return result;
    }
}