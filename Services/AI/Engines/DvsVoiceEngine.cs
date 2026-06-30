using DnDVoiceStudio.Services.AI.Runtime;
using System.IO;

namespace DnDVoiceStudio.Services.AI.Engines;

public sealed class DvsVoiceEngine : AiVoiceEngineBase
{
    public override bool LoadModel(string modelFolder)
    {
        BeginLoading();

        try
        {
            string? dvs = Directory
                .GetFiles(modelFolder, "*.dvsmodel")
                .FirstOrDefault();

            if (dvs is null)
            {
                Fail();
                return false;
            }
            UseBackend(new DummyInferenceBackend());

            if (!Runtime.Initialize())
            {
                Fail();
                return false;
            }
            FinishLoading();
            return true;
        }
        catch
        {
            Fail();
            return false;
        }
    }

    public override void Unload()
    {
        Runtime.Shutdown();
        Reset();
    }

    public override float[] Process(float[] samples)
    {
        return samples;
    }
}