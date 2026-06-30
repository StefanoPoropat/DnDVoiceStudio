using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace DnDVoiceStudio.Services.AI.Runtime;

public sealed class OnnxInferenceBackend : IInferenceBackend, IDisposable
{
    private InferenceSession? _session;

    public bool Initialize()
    {
        return _session != null;
    }

    public void LoadModel(string modelPath)
    {
        _session?.Dispose();
        _session = new InferenceSession(modelPath);
    }

    public float[] Process(float[] input)
    {
        if (_session == null)
            return input;

        // Convert to tensor [1, N]
        var tensor = new DenseTensor<float>(
            input,
            new[] { 1, input.Length });

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), tensor)
        };

        using var results = _session.Run(inputs);

        var output = results.First().AsTensor<float>().ToArray();
        System.Diagnostics.Debug.WriteLine(
    $"ONNX IN: {input.Length}");
        System.Diagnostics.Debug.WriteLine(
    $"ONNX OUT: {output.Length}");
        return output;
    }

    public void Shutdown()
    {
        _session?.Dispose();
        _session = null;
    }

    public void Dispose()
    {
        Shutdown();
    }
}