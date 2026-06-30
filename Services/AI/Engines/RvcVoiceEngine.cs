using DnDVoiceStudio.Services.Ai.Common;
using DnDVoiceStudio.Services.AI.Common;
using DnDVoiceStudio.Services.AI.Runtime;
using DnDVoiceStudio.Services.AI.RVC;

namespace DnDVoiceStudio.Services.AI.Engines;

public sealed class RvcVoiceEngine : AiVoiceEngineBase
{
    private readonly RvcModelLoader _loader =
        new();

    private RvcInferenceContext? _context;

    public override bool LoadModel(string modelFolder)
    {
        BeginLoading();

        try
        {
            var model =
                _loader.Load(modelFolder);

            if (model == null)
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

            _context =
                new RvcInferenceContext
                {
                    Model = model
                };

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
        _context = null;
        Runtime.Shutdown();
        Reset();
    }

    public override float[] Process(float[] samples)
    {
        if (State != AiEngineState.Loaded)
            return samples;

        if (_context == null)
            return samples;

        _context.Pipeline.Push(samples);

        while (_context.Pipeline.HasFrame)
        {
            AudioFrame frame =
                _context.Pipeline.GetFrame();

            var timer =
    BeginInference();

            float[] processed =
                _context.Preprocessor.Process(
                    frame.Samples);

            processed = Runtime.Run(processed);

            processed =
                _context.Postprocessor.Process(
                    processed);

            EndInference(
                timer,
                processed.Length);

            _context.Pipeline.AddProcessedFrame(
                processed);
        }

        return _context.Pipeline.GetOutput();
    }
}