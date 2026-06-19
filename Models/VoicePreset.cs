using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDVoiceStudio.Models;

public partial class VoicePreset : ObservableObject
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private float pitch;

    [ObservableProperty]
    private float formant;

    [ObservableProperty]
    private float bassBoost;

    [ObservableProperty]
    private float trebleBoost;

    [ObservableProperty]
    private float compression;

    [ObservableProperty]
    private float reverb;

    [ObservableProperty]
    private float distortion;

    [ObservableProperty]
    private bool isAiVoice;

    [ObservableProperty]
    private float demon;

    [ObservableProperty]
    private float whisper;

    [ObservableProperty]
    private float radio;

    [ObservableProperty]
    private float titan;

    [ObservableProperty]
    private string hotkey = "";
}