namespace DnDVoiceStudio.Services.Audio;

public class VoiceProfile
{
    public string Name { get; set; } = "Narrator";

    public float Pitch { get; set; }

    public float Reverb { get; set; }

    public float Distortion { get; set; }

    public bool IsAiVoice { get; set; }

    public float Formant
    {
        get;
        set;
    }

    public float BassBoost
    {
        get;
        set;
    }

    public float TrebleBoost
    {
        get;
        set;
    }

    public float Compression
    {
        get;
        set;
    }

    public float Demon
    {
        get;
        set;
    }

    public float Whisper
    {
        get;
        set;
    }

    public float Radio
    {
        get;
        set;
    }

    public float Titan
    {
        get;
        set;
    }
}