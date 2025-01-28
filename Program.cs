using System;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;
using NAudio.CoreAudioApi;

class Program
{
    // Variable to store the default device ID
    static string defaultDeviceID = null;

    static void Main(string[] args)
    {
        if (args.Length == 2)
        {
            string deviceNamePattern = args[0].ToLower();
            if (!int.TryParse(args[1], out int volumeLevel) || volumeLevel < 0 || volumeLevel > 100)
            {
                Console.WriteLine("Invalid volume level. Please provide a level between 0 and 100.", Color.Red);
                return;
            }

            SetVolumeByPattern(deviceNamePattern, volumeLevel);
        }
        else
        {
            RunInteractiveMode();
        }
    }

    static void SetVolumeByPattern(string deviceNamePattern, int volumeLevel)
    {
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                                .Where(d => d.FriendlyName.ToLower().Contains(deviceNamePattern))
                                .ToList();

        if (devices.Count == 0)
        {
            Console.WriteLine($"No devices found matching pattern '{deviceNamePattern}'", Color.Red);
            return;
        }
        else if (devices.Count == 1)
        {
            SetVolume(devices[0], volumeLevel);
        }
        else
        {
            Console.WriteLine("Multiple devices found:", Color.Yellow);
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {devices[i].FriendlyName}", Color.Yellow);
            }

            Console.Write("Select device number: ", Color.Green);
            string input = Console.ReadLine();
            if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= devices.Count)
            {
                SetVolume(devices[selectedIndex - 1], volumeLevel);
            }
            else
            {
                Console.WriteLine("Invalid selection.", Color.Red);
            }
        }
    }

    static void RunInteractiveMode()
    {
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

        while (true)
        {
            Console.WriteLine("Available playback devices:", Color.Green);
            for (int i = 0; i < devices.Count; i++)
            {
                int currentVolume = (int)(devices[i].AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                // Compare based on the device ID
                string defaultMark = (defaultDeviceID == devices[i].ID) ? " [DEFAULT]" : "";
                Console.WriteLine($"{i + 1}. {devices[i].FriendlyName} (Current Volume: {currentVolume}%) {defaultMark}", Color.Yellow);
            }
            Console.WriteLine("\nTo set a device as default, type 'default [NUMBER]'.");
            Console.Write("[input number device follow with level, example: 2 20] or command: ", Color.Green);
            string input = Console.ReadLine();

            // Parse the input
            string[] parts = input.Split(' ');
            if (parts.Length == 2 && parts[0].ToLower() == "default" && int.TryParse(parts[1], out int deviceIndex))
            {
                if (deviceIndex > 0 && deviceIndex <= devices.Count)
                {
                    defaultDeviceID = devices[deviceIndex - 1].ID;  // Set default device by ID
                    Console.WriteLine($"Device {devices[deviceIndex - 1].FriendlyName} set as default.", Color.LightGreen);
                }
                else
                {
                    Console.WriteLine("Invalid device index.", Color.Red);
                }
            }
            else if (parts.Length == 2 && int.TryParse(parts[0], out deviceIndex) && int.TryParse(parts[1], out int volumeLevel))
            {
                if (deviceIndex > 0 && deviceIndex <= devices.Count && volumeLevel >= 0 && volumeLevel <= 100)
                {
                    MMDevice selectedDevice = devices[deviceIndex - 1];
                    float volumeScalar = volumeLevel / 100f;
                    selectedDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volumeScalar;
                    Console.WriteLine($"Volume set to {volumeLevel}% for device {selectedDevice.FriendlyName}", Color.LightGreen);
                }
                else
                {
                    Console.WriteLine("Invalid device index or volume level.", Color.Red);
                }
            }
            else
            {
                Console.WriteLine("Invalid device index or volume level.", Color.Red);
            }

            // Clear the console before re-displaying the devices list with the updated default device
            Console.Clear();
        }
    }

    static void SetVolume(MMDevice device, int volumeLevel)
    {
        float volumeScalar = volumeLevel / 100f;
        device.AudioEndpointVolume.MasterVolumeLevelScalar = volumeScalar;
        Console.WriteLine($"Volume set to {volumeLevel}% for device {device.FriendlyName}", Color.LightGreen);
    }
}

