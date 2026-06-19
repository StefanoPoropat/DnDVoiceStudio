using System.Diagnostics;

namespace DnDVoiceStudio.Services.AI;

public class PythonEnvironmentService
{
    public bool IsPythonInstalled()
    {
        try
        {
            Process process = new();

            process.StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();

            process.WaitForExit(3000);

            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    public string GetPythonVersion()
    {
        try
        {
            Process process = new();

            process.StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.Start();

            string output =
                process.StandardOutput.ReadToEnd();

            string error =
                process.StandardError.ReadToEnd();

            process.WaitForExit();

            return string.IsNullOrWhiteSpace(output)
                ? error
                : output;
        }
        catch
        {
            return "Python not found";
        }
    }
}