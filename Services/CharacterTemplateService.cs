using DnDVoiceStudio.Models;

namespace DnDVoiceStudio.Services;

public class CharacterTemplateService
{
    public List<CharacterTemplate>
        GetTemplates()
    {
        return new()
        {
            new CharacterTemplate
            {
                Name = "Narrator"
            },

            new CharacterTemplate
            {
                Name = "Goblin",
                Pitch = 5,
                Formant = 6,
                Compression = 40
            },

            new CharacterTemplate
            {
                Name = "Dragon",
                Pitch = -4,
                Formant = -5,
                BassBoost = 30,
                Titan = 70,
                Demon = 30
            },

            new CharacterTemplate
            {
                Name = "Lich",
                Pitch = -2,
                Whisper = 40,
                Reverb = 20
            },

            new CharacterTemplate
            {
                Name = "Titan",
                Pitch = -3,
                BassBoost = 40,
                Titan = 100
            },

            new CharacterTemplate
            {
                Name = "Ghost",
                Whisper = 50,
                Reverb = 35
            },

            new CharacterTemplate
            {
                Name = "Military Radio",
                Radio = 80,
                Compression = 60
            }
        };
    }
}