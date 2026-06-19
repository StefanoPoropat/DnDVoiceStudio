namespace DnDVoiceStudio.Services.Audio.Effects;

public class FormantShiftEffect
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
        if (Math.Abs(_amount) < 0.01f)
            return samples;

        float gain =
            1f + (_amount * 0.03f);

        float[] result =
            new float[samples.Length];

        for (int i = 0;
             i < samples.Length;
             i++)
        {
            result[i] =
                Math.Clamp(
                    samples[i] * gain,
                    -1f,
                    1f);
        }

        return result;
    }
}