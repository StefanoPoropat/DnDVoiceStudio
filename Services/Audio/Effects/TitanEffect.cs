namespace DnDVoiceStudio.Services.Audio.Effects;

public class TitanEffect
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
        if (_amount <= 0)
            return samples;

        float mix =
            _amount / 100f;

        float[] result =
            new float[samples.Length];

        int delaySamples =
            (int)(60 * mix);

        for (int i = 0;
             i < samples.Length;
             i++)
        {
            float clean =
                samples[i];

            float giant =
                clean * (1f + mix);

            if (i >= delaySamples)
            {
                giant +=
                    samples[i - delaySamples]
                    * 0.5f
                    * mix;
            }

            result[i] =
                Math.Clamp(
                    clean * (1f - mix)
                    + giant * mix,
                    -1f,
                    1f);
        }

        return result;
    }

}