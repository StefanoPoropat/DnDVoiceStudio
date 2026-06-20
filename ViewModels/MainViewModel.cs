using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDVoiceStudio.Services.Ai;
using DnDVoiceStudio.Services.Ai.Environment;
using DnDVoiceStudio.Services.AI;
using DnDVoiceStudio.Services.Audio;
using System.Collections.ObjectModel;
namespace DnDVoiceStudio.ViewModels;
using DnDVoiceStudio.Models;
using DnDVoiceStudio.Services;
using DnDVoiceStudio.Services.VoicePreview;
using System.IO;

public partial class MainViewModel : ObservableObject
{
    private readonly PresetService _presetService;
    private readonly AudioEngine _audioEngine;

    private readonly SoundboardService _soundboardService =
        new();

    private readonly SoundEffectPlayer _soundPlayer =
        new();

    private readonly IVoicePreviewService
    _voicePreviewService;

    private List<SoundboardItem> _allSounds =
        new();

    public event Action<double>?
    LevelUpdated;

    [ObservableProperty]
    private VoicePreset? currentVoice;

    [ObservableProperty]
    private AudioDevice? selectedInputDevice;

    [ObservableProperty]
    private float microphoneLevel;

    [ObservableProperty]
    private bool isAudioRunning;

    [ObservableProperty]
    private float outputVolume = 4.0f;

    public ObservableCollection<VoicePreset> VoicePresets
    { get; }
        = new();

    public ObservableCollection<AudioDevice> InputDevices
    { get; }
        = new();

    public ObservableCollection<SoundboardItem>
        SoundboardItems
    { get; }
        = new();

    private readonly CharacterTemplateService
    _templateService = new();

    public ObservableCollection<CharacterTemplate>
    CharacterTemplates
    { get; }
    = new();

    private readonly VoiceMorphService
    _morphService = new();

    private readonly NpcLibraryService
    _npcService = new();

    public ObservableCollection<NpcProfile>
    Npcs
    { get; }
    = new();

    [ObservableProperty]
    private NpcProfile? activeNpc;

    //------------------------------------------------------------------------

    private float _currentDemon;
    public float CurrentDemon
    {
        get => _currentDemon;
        set
        {
            if (_currentDemon == value)
                return;

            _currentDemon = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentWhisper;
    public float CurrentWhisper
    {
        get => _currentWhisper;
        set
        {
            if (_currentWhisper == value)
                return;

            _currentWhisper = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentRadio;
    public float CurrentRadio
    {
        get => _currentRadio;
        set
        {
            if (_currentRadio == value)
                return;

            _currentRadio = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentTitan;
    public float CurrentTitan
    {
        get => _currentTitan;
        set
        {
            if (_currentTitan == value)
                return;

            _currentTitan = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private string _soundSearch = "";

    public string SoundSearch
    {
        get => _soundSearch;
        set
        {
            if (_soundSearch == value)
                return;

            _soundSearch = value;

            OnPropertyChanged();

            RefreshSoundList();
        }
    }

    public MainViewModel()
    {
        _presetService = new PresetService();

        _audioEngine = new AudioEngine();

        _voicePreviewService = new VoicePreviewService(_audioEngine);

        LoadPresets();

        LoadAudioDevices();

        LoadSounds();

        RefreshAiModels();

        LoadNpcs();


        _audioEngine.LevelChanged += level =>
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MicrophoneLevel = (float)level;

                System.Diagnostics.Debug.WriteLine(
                    $"Mic Level: {level}");
            });

            LevelUpdated?.Invoke(level);
        };

        _audioEngine.SetOutputVolume(
            OutputVolume);

        _audioEngine.SetAiEngine(
            _dummyAi);

        foreach (var template in
         _templateService.GetTemplates())
        {
            CharacterTemplates.Add(
                template);
        }

        MorphPresetA =
    VoicePresets.FirstOrDefault();

        MorphPresetB =
            VoicePresets.Skip(1)
                        .FirstOrDefault();

    }

    private void LoadPresets()
    {
        var presets =
            _presetService.LoadPresets();

        foreach (var preset in presets)
            VoicePresets.Add(preset);

        CurrentVoice = VoicePresets.FirstOrDefault();

        if (CurrentVoice != null)
        {
            PresetName = CurrentVoice.Name;
        }

        if (CurrentVoice != null)
        {
            CurrentPitch = CurrentVoice.Pitch;
            CurrentReverb = CurrentVoice.Reverb;
            CurrentDistortion = CurrentVoice.Distortion;

            UpdateCurrentVoice();
        }
    }

    private void LoadAudioDevices()
    {
        var manager =
            new AudioDeviceManager();

        foreach (var device in manager
                     .GetInputDevices())
        {
            InputDevices.Add(device);
        }

        SelectedInputDevice =
            InputDevices.FirstOrDefault();
    }

    private void LoadSounds()
    {
        _allSounds =
            _soundboardService.LoadSounds();

        RefreshSoundList();
    }

    private void RefreshSoundList()
    {
        SoundboardItems.Clear();

        IEnumerable<SoundboardItem> results =
            _allSounds;

        if (!string.IsNullOrWhiteSpace(
                SoundSearch))
        {
            results =
                results.Where(
                    s => s.Name.Contains(
                        SoundSearch,
                        StringComparison
                            .OrdinalIgnoreCase));
        }

        foreach (var sound in results)
        {
            SoundboardItems.Add(sound);
        }
    }

    partial void OnOutputVolumeChanged(
        float value)
    {
        _audioEngine.SetOutputVolume(
            value);
    }

    private float _currentPitch;
    public float CurrentPitch
    {
        get => _currentPitch;
        set
        {
            if (_currentPitch == value)
                return;

            _currentPitch = value;

            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentReverb;
    public float CurrentReverb
    {
        get => _currentReverb;
        set
        {
            if (_currentReverb == value)
                return;

            _currentReverb = value;

            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentDistortion;
    public float CurrentDistortion
    {
        get => _currentDistortion;
        set
        {
            if (_currentDistortion == value)
                return;

            _currentDistortion = value;

            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    [RelayCommand]
    private void StartMic()
    {
        if (SelectedInputDevice == null)
            return;

        _audioEngine.Start(
            SelectedInputDevice.DeviceNumber);

        IsAudioRunning = true;
    }

    [RelayCommand]
    private void StopMic()
    {
        _audioEngine.Stop();

        IsAudioRunning = false;
    }

    [RelayCommand]
    private void SelectVoice(VoicePreset preset)
    {
        if (preset == null)
            return;

        CurrentVoice = preset;
        PresetName = preset.Name;

        CurrentPitch = preset.Pitch;
        CurrentFormant = preset.Formant;
        CurrentBassBoost = preset.BassBoost;
        CurrentTrebleBoost = preset.TrebleBoost;
        CurrentCompression = preset.Compression;
        CurrentReverb = preset.Reverb;
        CurrentDistortion = preset.Distortion;
        CurrentDemon = preset.Demon;
        CurrentWhisper = preset.Whisper;
        CurrentRadio = preset.Radio;
        CurrentTitan = preset.Titan;

        UpdateCurrentVoice();
    }

    [RelayCommand]
    private void PlaySound(SoundboardItem item)
    {
        if (item == null)
            return;

        _soundPlayer.Play(item.FilePath);
    }

    [RelayCommand]
    private void StopAllSounds()
    {
        _soundPlayer.StopAll();
    }

    private void UpdateCurrentVoice()
    {
        _audioEngine.SetProfile(
    new VoiceProfile
    {
        Name =
            CurrentVoice?.Name ??
            "Custom",

        Pitch =
            CurrentPitch,

        Formant =
            CurrentFormant,

        BassBoost =
            CurrentBassBoost,

        TrebleBoost =
            CurrentTrebleBoost,

        Compression =
            CurrentCompression,

        Reverb =
            CurrentReverb,

        Distortion =
            CurrentDistortion,

        IsAiVoice =
            false,

        Demon = CurrentDemon,
        Whisper = CurrentWhisper,
        Radio = CurrentRadio,
        Titan = CurrentTitan,
    });
    }

    [RelayCommand]
    private void SavePreset()
    {
        if (CurrentVoice == null)
            return;

        CurrentVoice.Name =
            PresetName;

        CurrentVoice.Pitch =
            CurrentPitch;

        CurrentVoice.Formant =
            CurrentFormant;

        CurrentVoice.BassBoost =
            CurrentBassBoost;

        CurrentVoice.TrebleBoost =
            CurrentTrebleBoost;

        CurrentVoice.Compression =
            CurrentCompression;

        CurrentVoice.Reverb =
            CurrentReverb;

        CurrentVoice.Distortion =
            CurrentDistortion;

        CurrentVoice.Demon = CurrentDemon;
        CurrentVoice.Whisper = CurrentWhisper;
        CurrentVoice.Radio = CurrentRadio;
        CurrentVoice.Titan = CurrentTitan;

        _presetService.SavePresets(
            VoicePresets);

        OnPropertyChanged(
            nameof(CurrentVoice));

        StatusMessage =
            $"Saved preset: {CurrentVoice.Name}";
    }
    [RelayCommand]
    private void DeletePreset()
    {
        if (CurrentVoice == null)
            return;

        VoicePresets.Remove(
            CurrentVoice);

        _presetService.SavePresets(
            VoicePresets);

        CurrentVoice =
            VoicePresets.FirstOrDefault();
    }
    [RelayCommand]
    private void NewPreset()
    {
        var preset = new VoicePreset
        {
            Name = $"Custom {VoicePresets.Count + 1}",

            Pitch = CurrentPitch,
            Formant = CurrentFormant,
            BassBoost = CurrentBassBoost,
            TrebleBoost = CurrentTrebleBoost,
            Compression = CurrentCompression,

            Reverb = CurrentReverb,
            Distortion = CurrentDistortion,

            Demon = CurrentDemon,
            Whisper = CurrentWhisper,
            Radio = CurrentRadio,
            Titan = CurrentTitan,

            IsAiVoice = false
        };

        VoicePresets.Add(preset);

        CurrentVoice = preset;

        PresetName = preset.Name;

        CurrentPitch = preset.Pitch;
        CurrentFormant = preset.Formant;
        CurrentBassBoost = preset.BassBoost;
        CurrentTrebleBoost = preset.TrebleBoost;
        CurrentCompression = preset.Compression;
        CurrentReverb = preset.Reverb;
        CurrentDistortion = preset.Distortion;
        CurrentDemon = preset.Demon;
        CurrentWhisper = preset.Whisper;
        CurrentRadio = preset.Radio;
        CurrentTitan = preset.Titan;

        _presetService.SavePresets(VoicePresets);
    }

    private string _presetName = "";

    public string PresetName
    {
        get => _presetName;
        set
        {
            if (_presetName == value)
                return;

            _presetName = value;

            OnPropertyChanged();
        }
    }
    public ObservableCollection<AiVoiceModel>
AiModels
    { get; }
    = new();

    private readonly AiModelService
    _aiModelService = new();

    [RelayCommand]
    private void RefreshAiModels()
    {
        AiModels.Clear();

        foreach (var model in
                 _aiModelService.LoadModels())
        {
            AiModels.Add(model);
        }
    }

    [RelayCommand]
    private void LoadAiModel(
    AiVoiceModel model)
    {
        if (model == null)
            return;

        bool loaded =
            _audioEngine.LoadAiModel(
                model.ModelPath);

        if (loaded)
        {
            LoadedAiModel =
                model.Name;

            StatusMessage =
                $"Loaded AI model: {model.Name}";
        }
        else
        {
            StatusMessage =
                $"Failed to load model";
        }
    }

    [ObservableProperty]
    private string statusMessage = "Ready";

    [ObservableProperty]
    private string loadedAiModel = "None";

    [RelayCommand]
    private void CheckPython()
    {
        var python =
            new PythonEnvironmentService();

        if (python.IsPythonInstalled())
        {
            StatusMessage =
                python.GetPythonVersion();
        }
        else
        {
            StatusMessage =
                "Python not installed";
        }
    }

    private readonly DummyAiVoiceEngine
    _dummyAi =
        new();

    private readonly AiEnvironmentService
    _environmentService =
        new();
    [RelayCommand]
    private void CheckAiEnvironment()
    {
        bool python =
            _environmentService
            .IsPythonInstalled();

        bool ffmpeg =
            _environmentService
            .IsFfmpegInstalled();

        StatusMessage =
            $"Python: {(python ? "OK" : "Missing")} | " +
            $"FFmpeg: {(ffmpeg ? "OK" : "Missing")}";
    }



    private float _currentFormant;
    public float CurrentFormant
    {
        get => _currentFormant;
        set
        {
            if (_currentFormant == value)
                return;

            _currentFormant = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentBassBoost;
    public float CurrentBassBoost
    {
        get => _currentBassBoost;
        set
        {
            if (_currentBassBoost == value)
                return;

            _currentBassBoost = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentTrebleBoost;
    public float CurrentTrebleBoost
    {
        get => _currentTrebleBoost;
        set
        {
            if (_currentTrebleBoost == value)
                return;

            _currentTrebleBoost = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }

    private float _currentCompression;
    public float CurrentCompression
    {
        get => _currentCompression;
        set
        {
            if (_currentCompression == value)
                return;

            _currentCompression = value;
            OnPropertyChanged();

            UpdateCurrentVoice();
        }
    }
    [RelayCommand]
    private async Task PreviewVoice()
    {
        if (CurrentVoice == null)
            return;

        await _voicePreviewService
            .PreviewAsync(
                CurrentVoice);
    }
    [RelayCommand]
    private void StopPreview()
    {
        _voicePreviewService.Stop();
    }

    [RelayCommand]
    private void ApplyTemplate(
    CharacterTemplate template)
    {
        if (template == null)
            return;

        CurrentPitch =
            template.Pitch;

        CurrentFormant =
            template.Formant;

        CurrentBassBoost =
            template.BassBoost;

        CurrentTrebleBoost =
            template.TrebleBoost;

        CurrentCompression =
            template.Compression;

        CurrentReverb =
            template.Reverb;

        CurrentDistortion =
            template.Distortion;

        CurrentDemon =
            template.Demon;

        CurrentWhisper =
            template.Whisper;

        CurrentRadio =
            template.Radio;

        CurrentTitan =
            template.Titan;

        StatusMessage =
            $"Applied template: {template.Name}";
    }
    [ObservableProperty]
    private VoicePreset? morphPresetA;

    [ObservableProperty]
    private VoicePreset? morphPresetB;

    [ObservableProperty]
    private float morphBlend = 0.5f;

    [RelayCommand]
    private void ApplyMorph()
    {
        if (MorphPresetA == null)
            return;

        if (MorphPresetB == null)
            return;

        VoicePreset result =
            _morphService.Morph(
                MorphPresetA,
                MorphPresetB,
                MorphBlend);

        CurrentPitch =
            result.Pitch;

        CurrentFormant =
            result.Formant;

        CurrentBassBoost =
            result.BassBoost;

        CurrentTrebleBoost =
            result.TrebleBoost;

        CurrentCompression =
            result.Compression;

        CurrentReverb =
            result.Reverb;

        CurrentDistortion =
            result.Distortion;

        CurrentDemon =
            result.Demon;

        CurrentWhisper =
            result.Whisper;

        CurrentRadio =
            result.Radio;

        CurrentTitan =
            result.Titan;

        StatusMessage =
            $"Morphed: {result.Name}";
    }

    [ObservableProperty]
    private NpcProfile? selectedNpc;

    partial void OnSelectedNpcChanged(
        NpcProfile? value)
    {
        PendingPortraitPath = null;

        OnPropertyChanged(
            nameof(CurrentNpcPortrait));
    }

    private void LoadNpcs()
    {
        Npcs.Clear();

        foreach (var npc in _npcService.LoadNpcs())
        {
            Npcs.Add(npc);
        }
        foreach (var npc in _npcService.LoadNpcs())
        {
            if (string.IsNullOrWhiteSpace(
                npc.PortraitPath))
            {
                npc.PortraitPath =
                    @"Assets\NPCPortraits\default.png";
            }
        }

        SelectedNpc =
            Npcs.FirstOrDefault();
        ActiveNpc = SelectedNpc;
        OnPropertyChanged(
    nameof(CurrentActiveNpcPortrait));
    }

    [RelayCommand]
    private void NewNpc()
    {
        var npc = new NpcProfile
        {
            Name = "New NPC",
            PortraitPath = @"Assets\NPCPortraits\default.png"
        };

        Npcs.Add(npc);

        SelectedNpc = npc;
    }

    [RelayCommand]
    private void SaveNpc()
    {
        if (SelectedNpc != null &&
            !string.IsNullOrWhiteSpace(
                PendingPortraitPath))
        {
            string portraitsFolder =
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "NPCPortraits");

            Directory.CreateDirectory(
                portraitsFolder);

            string extension =
                Path.GetExtension(
                    PendingPortraitPath);

            string fileName =
                $"{Guid.NewGuid()}{extension}";

            string destination =
                Path.Combine(
                    portraitsFolder,
                    fileName);

            File.Copy(
                PendingPortraitPath,
                destination,
                true);

            SelectedNpc.PortraitPath =
                Path.Combine(
                    "Assets",
                    "NPCPortraits",
                    fileName);

            PendingPortraitPath = null;
        }

        _npcService.SaveNpcs(Npcs);

        StatusMessage =
            $"Saved {Npcs.Count} NPCs";

        OnPropertyChanged(
            nameof(CurrentNpcPortrait));
    }
    [RelayCommand]
    private void DeleteNpc()
    {
        if (SelectedNpc == null)
            return;

        Npcs.Remove(
            SelectedNpc);

        SelectedNpc =
            Npcs.FirstOrDefault();

        _npcService.SaveNpcs(Npcs);
    }
    [RelayCommand]
    private void ActivateNpc()
    {
        if (SelectedNpc == null)
            return;

        var preset =
            VoicePresets.FirstOrDefault(
                p => p.Name ==
                     SelectedNpc.PresetName);

        if (preset == null)
            return;

        SelectVoice(preset);

        ActiveNpc = SelectedNpc;

        OnPropertyChanged(
            nameof(CurrentActiveNpcPortrait));

        StatusMessage =
            $"Activated NPC: {SelectedNpc.Name}";
    }

    public string CurrentActiveNpcPortrait
    {
        get
        {
            if (ActiveNpc == null ||
                string.IsNullOrWhiteSpace(
                    ActiveNpc.PortraitPath))
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "NPCPortraits",
                    "default.png");
            }

            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                ActiveNpc.PortraitPath);
        }
    }

    [RelayCommand]
    private void BrowsePortrait()
    {
        if (SelectedNpc == null)
            return;

        var dialog =
            new Microsoft.Win32.OpenFileDialog();

        dialog.Filter =
            "Images|*.png;*.jpg;*.jpeg";

        if (dialog.ShowDialog() == true)
        {

            string portraitsFolder =
    Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "Assets",
        "NPCPortraits");

            Directory.CreateDirectory(
                portraitsFolder);

            string fileName =
                Path.GetFileName(
                    dialog.FileName);

            string destination =
                Path.Combine(
                    portraitsFolder,
                    fileName);

            PendingPortraitPath =
    dialog.FileName;

            OnPropertyChanged(
                nameof(CurrentNpcPortrait));


        }
    }

    public string CurrentNpcPortrait
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(
                    PendingPortraitPath) &&
                File.Exists(
                    PendingPortraitPath))
            {
                return PendingPortraitPath;
            }

            if (SelectedNpc == null)
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "NPCPortraits",
                    "default.png");
            }

            if (string.IsNullOrWhiteSpace(
                SelectedNpc.PortraitPath))
            {
                return Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "NPCPortraits",
                    "default.png");
            }

            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                SelectedNpc.PortraitPath);
        }
    }

    [ObservableProperty]
    private string? pendingPortraitPath;

}