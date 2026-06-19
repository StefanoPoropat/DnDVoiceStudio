namespace DnDVoiceStudio.Services.Audio.Effects;

public class CompressionEffect
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

        float threshold =
            0.8f;

        float ratio =
            1f +
            (_amount / 20f);

        float[] output =
            new float[samples.Length];

        for (int i = 0; i < samples.Length; i++)
        {
            float sample =
                samples[i];

            float abs =
                Math.Abs(sample);

            if (abs > threshold)
            {
                float excess =
                    abs - threshold;

                excess /= ratio;

                sample =
                    Math.Sign(sample) *
                    (threshold + excess);
            }

            output[i] =
                sample;
        }

        return output;
    }
}