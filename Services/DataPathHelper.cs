using System.IO;

namespace DnDVoiceStudio.Services;

public static class DataPathHelper
{
    public static string ProjectRoot =>
        Directory.GetParent(
            AppDomain.CurrentDomain.BaseDirectory)!
        .Parent!
        .Parent!
        .Parent!
        .FullName;

    public static string DataFolder =>
        Path.Combine(
            ProjectRoot,
            "Data");

    public static string PortraitFolder =>
        Path.Combine(
            ProjectRoot,
            "NPCPortraits");

    public static string SoundsFolder =>
        Path.Combine(
            ProjectRoot,
            "Sounds");
    public static string VoiceModelsFolder =>
    Path.Combine(
        ProjectRoot,
        "VoiceModels");

    static DataPathHelper()
    {
        Directory.CreateDirectory(
            DataFolder);

        Directory.CreateDirectory(
            PortraitFolder);

        Directory.CreateDirectory(
            VoiceModelsFolder);
    }

    public static string GetDataFile(
        string fileName)
    {
        return Path.Combine(
            DataFolder,
            fileName);
    }
}