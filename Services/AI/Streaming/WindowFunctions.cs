namespace DnDVoiceStudio.Services.AI.Streaming;

public static class WindowFunctions
{
    public static float[] Hann(int length)
    {
        float[] window =
            new float[length];

        for (int i = 0; i < length; i++)
        {
            window[i] =
                0.5f *
                (1f -
                 MathF.Cos(
                     2f *
                     MathF.PI *
                     i /
                     (length - 1)));
        }

        return window;
    }

    public static void Apply(
        Span<float> samples,
        ReadOnlySpan<float> window)
    {
        int count =
            Math.Min(
                samples.Length,
                window.Length);

        for (int i = 0; i < count; i++)
        {
            samples[i] *=
                window[i];
        }
    }
}