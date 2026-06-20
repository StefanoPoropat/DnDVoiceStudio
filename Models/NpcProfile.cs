using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDVoiceStudio.Models;

public partial class NpcProfile : ObservableObject
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string description = "";

    [ObservableProperty]
    private string presetName = "";

    [ObservableProperty]
    private string hotkey = "";

    [ObservableProperty]
    private string portraitPath = "";
}