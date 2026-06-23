using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDVoiceStudio.Models;

public partial class SoundboardItem : ObservableObject
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string filePath = "";

    [ObservableProperty]
    private string category = "General";

    [ObservableProperty]
    private bool isFavorite;

    [ObservableProperty]
    private string hotkey = "";

    [ObservableProperty]
    private bool loop;

    [ObservableProperty]
    private float volume = 1.0f;
}