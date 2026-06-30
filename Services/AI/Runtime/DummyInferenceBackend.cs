namespace DnDVoiceStudio.Services.AI.Runtime;

public sealed class DummyInferenceBackend : IInferenceBackend
{
    public bool Initialize() => true;

    public void Shutdown() { }

    public float[] Process(float[] input)
    {
        float[] output = new float[input.Length];
        Array.Copy(input, output, input.Length);
        return output;
    }
}