namespace DnDVoiceStudio.Services.Audio.Effects;

public class FormantShiftEffect
{
    private float _amount;

    private float _lowState;
    private float _midState;

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

        float shift =
            _amount / 12f;

        float[] output =
            new float[samples.Length];

        for (int i = 0;
             i < samples.Length;
             i++)
        {
            float sample =
                samples[i];

            _lowState =
                (_lowState * 0.98f)
                + (sample * 0.02f);

            float low =
                _lowState;

            _midState =
                (_midState * 0.90f)
                + (sample * 0.10f);

            float mid =
                _midState - low;

            float high =
                sample - _midState;

            float lowGain =
                1f - (shift * 0.5f);

            float midGain =
                1f;

            float highGain =
                1f + (shift * 1.5f);

            float result =
                (low * lowGain)
                + (mid * midGain)
                + (high * highGain);

            output[i] =
                Math.Clamp(
                    result,
                    -1f,
                    1f);
        }

        return output;
    }
}