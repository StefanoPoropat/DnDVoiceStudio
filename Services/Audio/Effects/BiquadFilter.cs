namespace DnDVoiceStudio.Services.Audio.Effects;

public class BiquadFilter
{
    private readonly float b0;
    private readonly float b1;
    private readonly float b2;
    private readonly float a1;
    private readonly float a2;

    private float x1;
    private float x2;
    private float y1;
    private float y2;

    public BiquadFilter(
        float b0,
        float b1,
        float b2,
        float a0,
        float a1,
        float a2)
    {
        this.b0 = b0 / a0;
        this.b1 = b1 / a0;
        this.b2 = b2 / a0;

        this.a1 = a1 / a0;
        this.a2 = a2 / a0;
    }

    public float Process(float sample)
    {
        float output =
            b0 * sample +
            b1 * x1 +
            b2 * x2 -
            a1 * y1 -
            a2 * y2;

        x2 = x1;
        x1 = sample;

        y2 = y1;
        y1 = output;

        return output;
    }
}