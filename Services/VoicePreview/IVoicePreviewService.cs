using DnDVoiceStudio.Models;

namespace DnDVoiceStudio.Services.VoicePreview;

public interface IVoicePreviewService
{
    Task PreviewAsync(
        VoicePreset preset);

    void Stop();
}