using CommunityToolkit.Mvvm.ComponentModel;

namespace DnDVoiceStudio.Models;

public partial class Campaign : ObservableObject
{
    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private string description = "";
}