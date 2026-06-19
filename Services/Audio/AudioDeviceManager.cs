using NAudio.Wave;

namespace DnDVoiceStudio.Services.Audio;

public class AudioDeviceManager
{
    public List<AudioDevice> GetInputDevices()
    {
        var devices = new List<AudioDevice>();

        for (int i = 0; i < WaveIn.DeviceCount; i++)
        {
            var caps = WaveIn.GetCapabilities(i);

            devices.Add(new AudioDevice
            {
                DeviceNumber = i,
                Name = caps.ProductName
            });
        }

        return devices;
    }
}