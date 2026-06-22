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
            "Assets",
            "NPCPortraits");

    public static string SoundsFolder =>
        Path.Combine(
            ProjectRoot,
            "Sounds");

    static DataPathHelper()
    {
        Directory.CreateDirectory(
            DataFolder);

        Directory.CreateDirectory(
            PortraitFolder);
    }

    public static string GetDataFile(
        string fileName)
    {
        return Path.Combine(
            DataFolder,
            fileName);
    }
}