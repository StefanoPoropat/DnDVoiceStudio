using DnDVoiceStudio.Services;
using DnDVoiceStudio.ViewModels;
using System.Windows;
using System.Windows.Input;

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

        _hotkeys.HotkeyPressed += OnHotkeyPressed;
    }

    private readonly HotkeyService
    _hotkeys = new();

    private void Window_KeyDown(
    object sender,
    KeyEventArgs e)
    {
        _hotkeys.HandleKey(e.Key);
    }

    private void OnHotkeyPressed(
    Key key)
    {
        if (DataContext is not
            MainViewModel vm)
            return;

        string keyName =
            key.ToString();

        var preset =
            vm.VoicePresets
              .FirstOrDefault(
                  p => p.Hotkey ==
                       keyName);

        if (preset == null)
            return;

        vm.SelectVoiceCommand
          .Execute(preset);
    }
}