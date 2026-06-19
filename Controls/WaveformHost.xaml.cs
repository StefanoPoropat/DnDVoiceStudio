using System.Windows.Controls;

namespace DnDVoiceStudio.Controls;

public partial class WaveformHost :
    UserControl
{
    public WaveformControl Waveform
    {
        get;
    }

    = new();

    public WaveformHost()
    {
        InitializeComponent();

        Host.Content =
            Waveform;
    }
}