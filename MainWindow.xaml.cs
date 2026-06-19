using DnDVoiceStudio.ViewModels;
using System.Windows;

namespace DnDVoiceStudio;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        WaveformView.Waveform.AddLevel(20);
        WaveformView.Waveform.AddLevel(40);
        WaveformView.Waveform.AddLevel(80);
        WaveformView.Waveform.AddLevel(30);
        WaveformView.Waveform.AddLevel(60);

        var vm =
            new MainViewModel();

        DataContext = vm;

        vm.LevelUpdated += level =>
        {
            Dispatcher.Invoke(() =>
            {
                WaveformView
                    .Waveform
                    .AddLevel(
                        (float)level);
            });
        };
    }
}