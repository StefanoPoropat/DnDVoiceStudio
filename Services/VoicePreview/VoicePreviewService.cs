using DnDVoiceStudio.Models;
using DnDVoiceStudio.Services.Audio;

namespace DnDVoiceStudio.Services.VoicePreview;

public class VoicePreviewService
    : IVoicePreviewService
{
    private readonly AudioEngine _audio;

    public VoicePreviewService(
        AudioEngine audio)
    {
        _audio = audio;
    }

    public async Task PreviewAsync(
        VoicePreset preset)
    {
        await _audio.PlayPreviewAsync(
            @"Assets\PreviewVoices\voice-message.wav");
    }

    public void Stop()
    {
        _audio.StopPreview();
    }
}