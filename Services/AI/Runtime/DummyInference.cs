namespace DnDVoiceStudio.Services.AI.Runtime;

public sealed class DummyInference
{
    public float[] Run(float[] samples)
    {
        float[] output =
            new float[samples.Length];

        Array.Copy(
            samples,
            output,
            samples.Length);

        return output;
    }
}