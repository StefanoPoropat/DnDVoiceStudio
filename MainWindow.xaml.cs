using DnDVoiceStudio.Services;
using DnDVoiceStudio.ViewModels;
using System.IO;
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
        PreviewKeyDown += MainWindow_PreviewKeyDown;
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

        var npc =
            vm.Npcs.FirstOrDefault(
                n => n.Hotkey == keyName);

        if (npc != null)
        {
            vm.SelectedNpc = npc;

            vm.ActivateNpcCommand
                .Execute(null);

            return;
        }

        var preset =
            vm.VoicePresets.FirstOrDefault(
                p => p.Hotkey == keyName);

        if (preset != null)
        {
            vm.SelectVoiceCommand
                .Execute(preset);
        }
    }
    private void MainWindow_PreviewKeyDown(
    object sender,
    KeyEventArgs e)
    {
        if (DataContext is not MainViewModel vm)
            return;

        vm.HandleSoundHotkey(
            e.Key.ToString());
    }

    private void Window_Drop(
    object sender,
    DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(
            DataFormats.FileDrop))
            return;

        var files =
            (string[])e.Data.GetData(
                DataFormats.FileDrop);

        foreach (var file in files)
        {
            if (Directory.Exists(file))
            {
                var vm =
                    DataContext as MainViewModel;

                vm?.ImportModelFolderCommand
                    .Execute(file);
            }
        }
    }

}