using DnDVoiceStudio.Models;
using DnDVoiceStudio.Services.Audio;
using DnDVoiceStudio.ViewModels;

namespace DnDVoiceStudio.Services.Voice;

public static class VoiceProfileMapper
{
    public static VoiceProfile FromViewModel(MainViewModel vm)
    {
        var preset = vm.CurrentVoice;

        return new VoiceProfile
        {
            Name = preset?.Name ?? vm.PresetName ?? "Custom",

            Pitch = vm.CurrentPitch,
            Formant = vm.CurrentFormant,

            BassBoost = vm.CurrentBassBoost,
            TrebleBoost = vm.CurrentTrebleBoost,
            Compression = vm.CurrentCompression,

            Reverb = vm.CurrentReverb,
            Distortion = vm.CurrentDistortion,

            Demon = vm.CurrentDemon,
            Whisper = vm.CurrentWhisper,
            Radio = vm.CurrentRadio,
            Titan = vm.CurrentTitan,

            IsAiVoice = preset?.IsAiVoice ?? false
        };
    }

    public static VoiceProfile FromPreset(VoicePreset preset)
    {
        return new VoiceProfile
        {
            Name = preset.Name,

            Pitch = preset.Pitch,
            Formant = preset.Formant,

            BassBoost = preset.BassBoost,
            TrebleBoost = preset.TrebleBoost,
            Compression = preset.Compression,

            Reverb = preset.Reverb,
            Distortion = preset.Distortion,

            Demon = preset.Demon,
            Whisper = preset.Whisper,
            Radio = preset.Radio,
            Titan = preset.Titan,

            IsAiVoice = preset.IsAiVoice
        };
    }

    public static VoicePreset FromProfile(VoiceProfile profile)
    {
        return new VoicePreset
        {
            Name = profile.Name,

            Pitch = profile.Pitch,
            Formant = profile.Formant,

            BassBoost = profile.BassBoost,
            TrebleBoost = profile.TrebleBoost,
            Compression = profile.Compression,

            Reverb = profile.Reverb,
            Distortion = profile.Distortion,

            Demon = profile.Demon,
            Whisper = profile.Whisper,
            Radio = profile.Radio,
            Titan = profile.Titan,

            IsAiVoice = profile.IsAiVoice
        };
    }
}