using DnDVoiceStudio.Models;

namespace DnDVoiceStudio.Services;

public class VoiceMorphService
{
    public VoicePreset Morph(
        VoicePreset a,
        VoicePreset b,
        float blend)
    {
        return new VoicePreset
        {
            Name =
                $"{a.Name}-{b.Name}",

            Pitch =
                Lerp(
                    a.Pitch,
                    b.Pitch,
                    blend),

            Formant =
                Lerp(
                    a.Formant,
                    b.Formant,
                    blend),

            BassBoost =
                Lerp(
                    a.BassBoost,
                    b.BassBoost,
                    blend),

            TrebleBoost =
                Lerp(
                    a.TrebleBoost,
                    b.TrebleBoost,
                    blend),

            Compression =
                Lerp(
                    a.Compression,
                    b.Compression,
                    blend),

            Reverb =
                Lerp(
                    a.Reverb,
                    b.Reverb,
                    blend),

            Distortion =
                Lerp(
                    a.Distortion,
                    b.Distortion,
                    blend),

            Demon =
                Lerp(
                    a.Demon,
                    b.Demon,
                    blend),

            Whisper =
                Lerp(
                    a.Whisper,
                    b.Whisper,
                    blend),

            Radio =
                Lerp(
                    a.Radio,
                    b.Radio,
                    blend),

            Titan =
                Lerp(
                    a.Titan,
                    b.Titan,
                    blend)
        };
    }

    private float Lerp(
        float a,
        float b,
        float t)
    {
        return a + ((b - a) * t);
    }
}