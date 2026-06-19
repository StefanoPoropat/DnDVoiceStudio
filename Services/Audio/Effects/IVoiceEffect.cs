namespace DnDVoiceStudio.Services.Audio.Effects;

public interface IAudioEffect
{
    float[] Process(float[] samples);
}