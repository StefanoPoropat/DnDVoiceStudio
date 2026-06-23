namespace DnDVoiceStudio.Services.Ai;

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