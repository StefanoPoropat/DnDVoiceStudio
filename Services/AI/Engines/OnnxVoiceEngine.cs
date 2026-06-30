using DnDVoiceStudio.Services.AI.Runtime;
using System.IO;

namespace DnDVoiceStudio.Services.AI.Engines;

public sealed class OnnxVoiceEngine : AiVoiceEngineBase
{

    public override bool LoadModel(string modelFolder)
    {
        BeginLoading();

        try
        {
            string? onnx =
                Directory.GetFiles(modelFolder, "*.onnx")
                    .FirstOrDefault();

            if (onnx == null)
            {
                Fail();
                return false;
            }

            var backend = new OnnxInferenceBackend();
            backend.LoadModel(onnx);

            UseBackend(backend);

            Runtime.SetBackend(backend);

            if (!Runtime.Initialize())
            {
                Fail();
                return false;
            }

            FinishLoading();   // IMPORTANT: sets IsLoaded = true
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
        Latency.Begin();

        if (!IsLoaded)
            return samples;

        float[] result =
            Runtime.Run(samples);

        Latency.End();

        return result;
    }
}