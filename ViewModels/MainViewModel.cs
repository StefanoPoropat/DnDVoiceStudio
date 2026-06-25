using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DnDVoiceStudio.Services.Ai.Environment;
using DnDVoiceStudio.Services.AI;
using DnDVoiceStudio.Services.Audio;
namespace DnDVoiceStudio.ViewModels;
using DnDVoiceStudio.Models;
using DnDVoiceStudio.Services;
using DnDVoiceStudio.Services.Ai;
using DnDVoiceStudio.Services.VoicePreview;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

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

    private readonly CampaignService
    _campaignService = new();

    public ObservableCollection<Campaign>
    Campaigns
    { get; } = new();

    public ObservableCollection<NpcProfile>
    FilteredNpcs
    { get; } = new();

    public ObservableCollection<SoundboardItem>
    FilteredSoundboardItems
    { get; } = new();

    public ObservableCollection<string>
AvailableHotkeys
    { get; } = new()
{
    "",

    "NumPad1",
    "NumPad2",
    "NumPad3",
    "NumPad4",
    "NumPad5",
    "NumPad6",
    "NumPad7",
    "NumPad8",
    "NumPad9",
    "NumPad0"
};
    [ObservableProperty]
    private double duckAmount = 0.5;

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
    private float _currentDragon;
    public float CurrentDragon
    {
        get => _currentDragon;
        set
        {
            if (_currentDragon == value)
                return;

            _currentDragon = value;
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

            RefreshSoundboardFilter();
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
        LoadSoundCategories();

        LoadNpcs();

        LoadCampaigns();

        LoadVoiceModels();

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
        _allSounds = _soundboardService.LoadSounds();

        foreach (var sound in _allSounds)
        {
            sound.PropertyChanged += Sound_PropertyChanged;
        }

        RefreshSoundList();
        RefreshSoundboardFilter();
    }

    private void Sound_PropertyChanged(
    object? sender,
    PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SoundboardItem.Volume) ||
            e.PropertyName == nameof(SoundboardItem.IsFavorite) ||
            e.PropertyName == nameof(SoundboardItem.Hotkey))
        {
            _soundboardService.SaveMetadata(_allSounds);
        }
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

    //SOUNDBOARD STARTS HERE

    [RelayCommand]
    private void PlaySound(SoundboardItem item)
    {
        if (item == null)
            return;
        _audioEngine.SetDuckMultiplier(0.5f);
        _soundPlayer.Play(
            item.FilePath,
            item.Volume * SoundboardVolume,
            item.Loop);
        System.Diagnostics.Debug.WriteLine(
    $"PLAY: {item.Name}");
    }

    [RelayCommand]
    private void StopSound(
    SoundboardItem item)
    {
        if (item == null)
            return;
        _soundPlayer.Stop(item.FilePath);
        _audioEngine.SetDuckMultiplier(
    1f);
    }

    [RelayCommand]
    private void StopAllSounds()
    {
        _soundPlayer.StopAll();
    }

    [ObservableProperty]
    private string selectedSoundCategory = "All";

    public ObservableCollection<string>
    SoundCategories
    { get; } = new();
    private void RefreshSoundboardFilter()
    {
        FilteredSoundboardItems.Clear();

        IEnumerable<SoundboardItem> items =
            SoundboardItems;

        if (!string.IsNullOrWhiteSpace(
            SoundSearch))
        {
            items =
                items.Where(x =>
                    x.Name.Contains(
                        SoundSearch,
                        StringComparison
                            .OrdinalIgnoreCase));
        }

        switch (SelectedSoundCategory)
        {
            case "Favorites":

                items =
                    items.Where(
                        x => x.IsFavorite);

                break;

            case "All":

                break;

            default:

                items =
                    items.Where(
                        x => x.Category ==
                             SelectedSoundCategory);

                break;
        }

        foreach (var item in items)
        {
            FilteredSoundboardItems.Add(
                item);
        }
    }

    partial void OnSelectedSoundCategoryChanged(
    string value)
    {
        RefreshSoundboardFilter();
    }
    [RelayCommand]
    private void ToggleFavorite(
     SoundboardItem item)
    {
        _soundboardService
            .SaveMetadata(
                SoundboardItems);

        RefreshSoundboardFilter();
    }

    [ObservableProperty]
    private float soundboardVolume = 1.0f;

    private void LoadSoundCategories()
    {
        SoundCategories.Clear();

        SoundCategories.Add("All");
        SoundCategories.Add("Favorites");

        var categories =
            SoundboardItems
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

        foreach (var category in categories)
        {
            SoundCategories.Add(category);
        }
    }

    public void HandleSoundHotkey(
    string key)
    {
        var sound =
            SoundboardItems.FirstOrDefault(
                s => string.Equals(
                    s.Hotkey,
                    key,
                    StringComparison.OrdinalIgnoreCase));

        if (sound == null)
            return;

        PlaySound(sound);
        System.Diagnostics.Debug.WriteLine(
    $"HOTKEY: {key}");
    }

    private void ValidateSoundHotkeys()
    {
        var usedKeys =
            new HashSet<string>();

        foreach (var sound in SoundboardItems)
        {
            if (string.IsNullOrWhiteSpace(
                sound.Hotkey))
            {
                continue;
            }

            if (!usedKeys.Add(
                sound.Hotkey))
            {
                StatusMessage =
                    $"Duplicate hotkey removed from {sound.Name}";

                sound.Hotkey = "";
            }
        }
    }

    [RelayCommand]
    private void SaveSoundboard()
    {
        ValidateSoundHotkeys();

        _soundboardService.SaveMetadata(
            SoundboardItems);

        StatusMessage =
            "Soundboard saved";
    }

    // TBD STARTS HERE

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
        Dragon = CurrentDragon,
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


    //AI MODEL MANAGEMENT STARTS HERE

    public ObservableCollection<AiModelInfo>
    AiModels
    { get; }
    = new();

    private readonly AiModelService
    _aiModelService = new();

    [RelayCommand]
    private void RefreshVoiceModels()
    {
        LoadVoiceModels();

        StatusMessage =
            $"Found {AiModels.Count} voice models.";
    }

    [RelayCommand]
    private void LoadVoiceModel(
    AiModelInfo model)
    {
        if (model == null)
            return;

        LoadedAiModel =
            model.Name;

        StatusMessage =
            $"Loaded {model.Name}";

        if (!string.IsNullOrWhiteSpace(
            model.OnnxPath))
        {
            _onnxEngine.LoadModel(
                model.OnnxPath);

            _audioEngine.SetAiEngine(
                _onnxEngine);
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

    //private readonly VoiceModelService _voiceModelService = new();
    private readonly OnnxVoiceEngine _onnxEngine = new();

    //public ObservableCollection<VoiceModel> VoiceModels { get; } = new();
    private void LoadVoiceModels()
    {
        string fullPath = DataPathHelper.VoiceModels;

        System.Diagnostics.Debug.WriteLine(
            $"VOICE MODELS PATH = {fullPath}");

        System.Diagnostics.Debug.WriteLine(
            $"EXISTS = {Directory.Exists(fullPath)}");

        AiModels.Clear();

        var loader = new VoiceModelLoader();

        var models = loader.LoadModels(fullPath);

        if (models == null)
            return;

        foreach (var model in models)
        {
            System.Diagnostics.Debug.WriteLine(
                $"FOUND MODEL = {model.Name}");

            var config =
                VoiceModelConfigLoader
                    .Load(
                        model.FolderPath);

            AiModels.Add(
                new AiModelInfo
                {
                    Name = model.Name,
                    ModelPath = model.FolderPath,
                    ModelType = model.ModelType,
                    Config = config
                });
        }
        ApplyModelFilter();
    }

    private readonly ModelDiscoveryService
    _modelDiscovery =
        new();
    private void ApplyModelFilter()
    {
        FilteredAiModels.Clear();

        IEnumerable<AiModelInfo> models = AiModels;

        if (!string.IsNullOrWhiteSpace(ModelSearch))
        {
            string search = ModelSearch.Trim();

            models = models.Where(m =>
                m.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                m.ModelType.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (m.Config?.Author?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (m.Config?.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (m.Config?.DisplayName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        foreach (var model in models)
            FilteredAiModels.Add(model);
    }
    partial void OnModelSearchChanged(string value)
    {
        ApplyModelFilter();
    }


    //AI MODEL MANAGEMENT ENDS HERE
    //AI MODEL MANIPULATION STARTS HERE

    private readonly VoiceModelManager _modelManager = new();

    [ObservableProperty]
    private AiModelInfo? selectedAiModel;

    [RelayCommand]
    private void NewModel(string? type)
    {
        string name =
            $"Model_{DateTime.Now:HHmmss}";

        string modelType =
            type ?? "EMPTY";

        _modelManager.CreateModel(name, modelType);

        LoadVoiceModels();

        StatusMessage =
            $"Created {name} ({modelType})";
    }

    [RelayCommand]
    private void DeleteModel()
    {
        if (SelectedAiModel == null)
            return;

        _modelManager.DeleteModel(
            SelectedAiModel.ModelPath);

        LoadVoiceModels();

        StatusMessage =
            "Model deleted";
    }

    [RelayCommand]
    private void DuplicateModel()
    {
        if (SelectedAiModel == null)
            return;

        string newName =
            SelectedAiModel.Name
            + "_Copy";

        _modelManager.DuplicateModel(
            SelectedAiModel.ModelPath,
            newName);

        LoadVoiceModels();

        StatusMessage =
            $"Created {newName}";
    }
    private readonly ModelImportService _modelImport = new();
    [RelayCommand]
    private void ImportModelFolder(string folderPath)
    {
        var model =
            _modelImport.ImportFolder(folderPath);

        if (model == null)
        {
            StatusMessage =
                "Import failed.";
            return;
        }

        AiModels.Add(
            new AiModelInfo
            {
                Name = model.Name,
                FolderPath = model.FolderPath,
                ModelType = model.ModelType,
                Config = VoiceModelConfigLoader.Load(model.FolderPath)
            });

        StatusMessage =
            $"Imported {model.Name}";
        LoadVoiceModels();
    }

    //AI MODEL MANIPULATION ENDS HERE
    //AI MODEL MANAGEMENT STARTS HERE

    [ObservableProperty]
    private string modelDisplayName = "";

    [ObservableProperty]
    private string modelAuthor = "";

    [ObservableProperty]
    private string modelDescription = "";

    [ObservableProperty]
    private string modelVersion = "";

    partial void OnSelectedAiModelChanged(
    AiModelInfo? value)
    {
        if (value?.Config == null)
            return;

        ModelDisplayName =
            value.Config.DisplayName;

        ModelAuthor =
            value.Config.Author;

        ModelDescription =
            value.Config.Description;

        ModelVersion =
            value.Config.Version;
    }

    [RelayCommand]
    private void SaveModelMetadata()
    {
        if (SelectedAiModel == null)
            return;

        var config =
            SelectedAiModel.Config
            ?? new VoiceModelConfig();

        config.DisplayName =
            ModelDisplayName;

        config.Author =
            ModelAuthor;

        config.Description =
            ModelDescription;

        config.Version =
            ModelVersion;

        VoiceModelConfigSaver.Save(
            SelectedAiModel.ModelPath,
            config);

        StatusMessage =
            "Metadata saved.";

        //LoadVoiceModels();
    }

    [ObservableProperty]
    private string modelSearch = "";

    public IReadOnlyList<string> ModelTypes { get; } =
    [
        "EMPTY",
    "RVC",
    "ONNX",
    "DVS"
    ];

    public ObservableCollection<AiModelInfo> FilteredAiModels { get; } = new();

    //AI MODEL MANAGEMENT ENDS HERE
    //VOICE PARAMS STARTS HERE

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

    //PRESTES MANIPULATION STARTS HERE

    [RelayCommand]
    private void DuplicatePreset()
    {
        if (CurrentVoice == null)
            return;

        var copy = new VoicePreset
        {
            Name = CurrentVoice.Name + " Copy",

            Pitch = CurrentVoice.Pitch,
            Formant = CurrentVoice.Formant,

            BassBoost = CurrentVoice.BassBoost,
            TrebleBoost = CurrentVoice.TrebleBoost,

            Compression = CurrentVoice.Compression,
            Reverb = CurrentVoice.Reverb,
            Distortion = CurrentVoice.Distortion,

            Demon = CurrentVoice.Demon,
            Whisper = CurrentVoice.Whisper,
            Radio = CurrentVoice.Radio,
            Titan = CurrentVoice.Titan,

            IsAiVoice = CurrentVoice.IsAiVoice,

            Hotkey = ""
        };

        VoicePresets.Add(copy);

        CurrentVoice = copy;

        PresetName = copy.Name;

        _presetService.SavePresets(
            VoicePresets);

        StatusMessage =
            $"Duplicated preset: {copy.Name}";
    }

    [RelayCommand]
    private void ExportPreset()
    {
        if (CurrentVoice == null)
            return;

        var dialog =
            new SaveFileDialog();

        dialog.Filter =
            "Preset File|*.json";

        dialog.FileName =
            CurrentVoice.Name + ".json";

        if (dialog.ShowDialog() != true)
            return;

        string json =
            JsonSerializer.Serialize(
                CurrentVoice,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            dialog.FileName,
            json);

        StatusMessage =
            $"Exported {CurrentVoice.Name}";
    }
    [RelayCommand]
    private void ImportPreset()
    {
        var dialog =
            new OpenFileDialog();

        dialog.Filter =
            "Preset File|*.json";

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            string json =
                File.ReadAllText(
                    dialog.FileName);

            var preset =
                JsonSerializer.Deserialize<
                    VoicePreset>(json);

            if (preset == null)
                return;

            VoicePresets.Add(
                preset);

            CurrentVoice =
                preset;

            _presetService.SavePresets(
                VoicePresets);

            StatusMessage =
                $"Imported {preset.Name}";
        }
        catch
        {
            StatusMessage =
                "Invalid preset file";
        }
    }

    [RelayCommand]
    private void ExportPresetPack()
    {
        var dialog =
            new SaveFileDialog();

        dialog.Filter =
            "Preset Pack|*.json";

        dialog.FileName =
            "PresetPack.json";

        if (dialog.ShowDialog() != true)
            return;

        string json =
            JsonSerializer.Serialize(
                VoicePresets,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

        File.WriteAllText(
            dialog.FileName,
            json);

        StatusMessage =
            $"Exported {VoicePresets.Count} presets";
    }

    [RelayCommand]
    private void ImportPresetPack()
    {
        var dialog =
            new OpenFileDialog();

        dialog.Filter =
            "Preset Pack|*.json";

        if (dialog.ShowDialog() != true)
            return;

        try
        {
            string json =
                File.ReadAllText(
                    dialog.FileName);

            var presets =
                JsonSerializer.Deserialize<
                    List<VoicePreset>>(json);

            if (presets == null)
                return;

            foreach (var preset in presets)
            {
                VoicePresets.Add(
                    preset);
            }

            _presetService.SavePresets(
                VoicePresets);

            StatusMessage =
                $"Imported {presets.Count} presets";
        }
        catch
        {
            StatusMessage =
                "Invalid preset pack";
        }
    }



    //NPC START HERE

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
            if (string.IsNullOrWhiteSpace(npc.PortraitPath))
            {
                npc.PortraitPath = "default.png";
            }
            else
            {
                npc.PortraitPath =
                    Path.GetFileName(
                        npc.PortraitPath);
            }

            Npcs.Add(npc);
        }


        SelectedNpc =
            Npcs.FirstOrDefault();
        ActiveNpc = SelectedNpc;
        OnPropertyChanged(
    nameof(CurrentActiveNpcPortrait));

        RefreshNpcFilter();
    }

    [RelayCommand]
    private void NewNpc()
    {
        var npc = new NpcProfile
        {
            Name = "New NPC",
            PortraitPath = "default.png",
            Campaign = SelectedCampaign?.Name ?? "All Campaigns",
            PresetName = "Narrator"
        };

        Npcs.Add(npc);

        RefreshNpcFilter();

        SelectedNpc = npc;
    }

    [RelayCommand]
    private void SaveNpc()
    {
        var currentNpc = SelectedNpc;

        if (SelectedNpc != null &&
            !string.IsNullOrWhiteSpace(
                PendingPortraitPath))
        {
            // portrait save code
            string portraitsFolder =
                DataPathHelper.PortraitFolder;

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

            SelectedNpc.PortraitPath = fileName;

            PendingPortraitPath = null;
        }

        _npcService.SaveNpcs(Npcs);

        StatusMessage =
            $"Saved {Npcs.Count} NPCs";

        OnPropertyChanged(
            nameof(CurrentNpcPortrait));

        RefreshNpcFilter();

        SelectedNpc = currentNpc;
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

        RefreshNpcFilter();
    }
    [RelayCommand]
    private void ActivateNpc(
    NpcProfile? npc)
    {
        npc ??= SelectedNpc;

        if (npc == null)
            return;

        var preset =
            VoicePresets.FirstOrDefault(
                p => p.Name ==
                     npc.PresetName);

        if (preset == null)
            return;

        ActiveNpc = npc;

        SelectVoice(preset);

        StatusMessage =
            $"Activated NPC: {npc.Name}";
    }

    public string CurrentActiveNpcPortrait
    {
        get
        {
            if (ActiveNpc == null)
            {
                return Path.Combine(
                    DataPathHelper.PortraitFolder,
                    "default.png");
            }

            if (string.IsNullOrWhiteSpace(
                ActiveNpc.PortraitPath))
            {
                return Path.Combine(
                    DataPathHelper.PortraitFolder,
                    "default.png");
            }

            var portrait = ActiveNpc.PortraitPath;

            System.Diagnostics.Debug.WriteLine(
                $"PORTRAIT RAW = [{portrait}]");

            return Path.Combine(
                DataPathHelper.PortraitFolder,
                portrait);
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
                DataPathHelper.PortraitFolder;

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
                    DataPathHelper.PortraitFolder,
                    "default.png");
            }

            if (string.IsNullOrWhiteSpace(
                SelectedNpc.PortraitPath))
            {
                return Path.Combine(
                    DataPathHelper.PortraitFolder,
                    "default.png");
            }

            return Path.Combine(
                DataPathHelper.PortraitFolder,
                SelectedNpc.PortraitPath);
        }
    }

    [ObservableProperty]
    private string? pendingPortraitPath;

    public ObservableCollection<NpcProfile>
    SessionNpcs => FilteredNpcs;

    partial void OnActiveNpcChanged(
    NpcProfile? value)
    {
        OnPropertyChanged(
            nameof(CurrentActiveNpcPortrait));
    }

    [ObservableProperty]
    private Campaign? selectedCampaign;

    partial void OnSelectedCampaignChanged(
    Campaign? value)
    {
        RefreshNpcFilter();
    }

    private void LoadCampaigns()
    {
        Campaigns.Clear();

        Campaigns.Add(
    new Campaign
    {
        Name = "All Campaigns"
    });

        foreach (var campaign in
                 _campaignService.LoadCampaigns())
        {
            Campaigns.Add(campaign);
        }

        SelectedCampaign =
            Campaigns.FirstOrDefault();
    }

    [RelayCommand]
    private void NewCampaign()
    {
        var campaign =
            new Campaign
            {
                Name = "New Campaign"
            };

        Campaigns.Add(campaign);

        SelectedCampaign =
            campaign;
    }

    [RelayCommand]
    private void SaveCampaigns()
    {
        _campaignService
            .SaveCampaigns(Campaigns);

        StatusMessage =
            $"Saved {Campaigns.Count} campaigns";
    }

    [RelayCommand]
    private void DeleteCampaign()
    {
        if (SelectedCampaign == null)
            return;

        Campaigns.Remove(
            SelectedCampaign);

        SelectedCampaign =
            Campaigns.FirstOrDefault();

        _campaignService
            .SaveCampaigns(Campaigns);
    }
    private void RefreshNpcFilter()
    {
        FilteredNpcs.Clear();

        if (SelectedCampaign == null ||
            SelectedCampaign.Name == "All Campaigns")
        {
            foreach (var npc in Npcs)
            {
                FilteredNpcs.Add(npc);
            }
        }
        else
        {
            foreach (var npc in Npcs)
            {
                if (npc.Campaign ==
                    SelectedCampaign.Name)
                {
                    FilteredNpcs.Add(npc);
                }
            }
        }
        if (SelectedNpc == null ||
            !FilteredNpcs.Contains(SelectedNpc))
        {
            SelectedNpc =
                FilteredNpcs.FirstOrDefault();

            ActiveNpc =
                SelectedNpc;
        }
    }

}