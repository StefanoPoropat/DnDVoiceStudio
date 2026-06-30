using DnDVoiceStudio.Services.AI.DVS;
using DnDVoiceStudio.Services.AI.Interfaces;
using DnDVoiceStudio.Services.AI.ONNX;
using DnDVoiceStudio.Services.AI.RVC;

namespace DnDVoiceStudio.Services.AI.Engines;

public static class ModelEngineFactory
{
    public static IAiVoiceEngine
        CreateEngine(
            IAiModel model)
    {
        return model switch
        {
            RvcModel =>
                new RvcVoiceEngine(),

            OnnxModel =>
                new OnnxVoiceEngine(),

            DvsModel =>
                new DvsVoiceEngine(),

            _ =>
                new DummyAiVoiceEngine()
        };
    }
}