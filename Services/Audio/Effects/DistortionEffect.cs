namespace DnDVoiceStudio.Services.Audio.Effects;

public class DistortionEffect : IAudioEffect
{
    private float _amount;

    public void SetAmount(float amount)
    {
        _amount = Math.Clamp(amount, 0f, 1f);
    }

    public float[] Process(float[] samples)
    {
        if (_amount <= 0)
            return samples;

        float drive =
            1f + (_amount * 20f);

        float[] output =
            new float[samples.Length];

        for (int i = 0; i < samples.Length; i++)
        {
            output[i] =
                MathF.Tanh(
                    samples[i] * drive);
        }

        return output;
    }
}