namespace DnDVoiceStudio.Services.Audio.Effects;

public class WhisperEffect
{
    private float _amount;

    public void SetAmount(
        float amount)
    {
        _amount = amount;
    }

    private readonly Random _random =
    new();

    public float[] Process(
    float[] samples)
    {
        if (_amount <= 0)
            return samples;

        float mix =
            _amount / 100f;



        float[] result =
            new float[samples.Length];

        for (int i = 0;
             i < samples.Length;
             i++)
        {
            float clean =
                samples[i];

            float noise =
                ((float)_random.NextDouble()
                - 0.5f)
                * 0.05f
                * mix;

            float whisper =
                clean * (1f - mix * 0.6f)
                + noise;

            result[i] =
                Math.Clamp(
                    clean * (1f - mix)
                    + whisper * mix,
                    -1f,
                    1f);
        }

        return result;
    }

}