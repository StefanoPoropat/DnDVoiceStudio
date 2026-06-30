using System.IO;

namespace DnDVoiceStudio.Services.AI.RVC;

public static class RvcModelValidator
{
    public static bool Validate(string folder)
    {
        if (!Directory.Exists(folder))
            return false;

        string pth =
            Path.Combine(folder, "model.pth");

        string config =
            Path.Combine(folder, "config.json");

        if (!File.Exists(pth))
            return false;

        if (!File.Exists(config))
            return false;

        try
        {
            _ = File.ReadAllText(config);
        }
        catch
        {
            return false;
        }

        return true;
    }
}