using System.Diagnostics;

namespace DnDVoiceStudio.Services.AI.Common;

public sealed class LatencyTracker
{
    private readonly Stopwatch _sw = new();

    private double _lastMs;

    public double LastLatencyMs => _lastMs;

    public void Begin()
    {
        _sw.Restart();
    }

    public void End()
    {
        _sw.Stop();
        _lastMs = _sw.Elapsed.TotalMilliseconds;
    }
}