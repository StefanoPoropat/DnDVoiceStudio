namespace DnDVoiceStudio.Services.Audio.Effects;

public class RadioEffect
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

        float previous = 0;

        for (int i = 0;
             i < samples.Length;
             i++)
        {
            float sample =
                samples[i];

            sample =
                (float)Math.Round(
                    sample * 16f)
                / 16f;

            sample =
                (sample * 0.8f)
                + (previous * 0.2f);

            previous = sample;

            result[i] =
                Math.Clamp(
                    samples[i] * (1f - mix)
                    + sample * mix,
                    -1f,
                    1f);
        }

        return result;
    }

}