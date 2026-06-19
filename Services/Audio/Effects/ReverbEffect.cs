namespace DnDVoiceStudio.Services.Audio.Effects;

public class ReverbEffect : IAudioEffect
{
    private readonly float[] _delayBuffer;

    private int _position;

    private float _amount;

    public ReverbEffect()
    {
        _delayBuffer = new float[16000];
    }

    public void SetAmount(float amount)
    {
        _amount = Math.Clamp(amount, 0f, 1f);
    }

    public float[] Process(float[] samples)
    {
        if (_amount <= 0)
            return samples;

        float[] output =
            new float[samples.Length];

        for (int i = 0; i < samples.Length; i++)
        {
            float delayed =
                _delayBuffer[_position];

            output[i] =
                samples[i] +
                delayed * _amount;

            _delayBuffer[_position] =
                samples[i] +
                delayed * 0.5f;

            _position++;

            if (_position >= _delayBuffer.Length)
                _position = 0;
        }

        return output;
    }
}