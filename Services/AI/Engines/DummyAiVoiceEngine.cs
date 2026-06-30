using DnDVoiceStudio.Services.AI.Runtime;

namespace DnDVoiceStudio.Services.AI.Engines;

public sealed class DummyAiVoiceEngine : AiVoiceEngineBase
{
    public override bool LoadModel(string modelPath)
    {
        UseBackend(new DummyInferenceBackend());

        if (!Runtime.Initialize())
        {
            Fail();
            return false;
        }

        FinishLoading();
        return true;
    }

    public override void Unload()
    {
        Reset();
    }

    public override float[] Process(float[] samples)
    {
        var timer = BeginInference();

        EndInference(
            timer,
            samples.Length);

        return samples;
    }
}