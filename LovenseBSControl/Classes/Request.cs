using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System.Linq;

namespace LovenseBSControl.Classes
{
    public class Request
    {
        private ButtplugClient _client = new ButtplugClient("Butt Saber");

        public Request()
        {
        }

        public async Task Connect()
        {
            await _client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345")));
            foreach (var d in _client.Devices)
            {
                await TestDevice(d);
            }
        }

        public IEnumerable<ButtplugClientDevice> RequestToysList()
        {
            return _client.Devices;
        }

        public async Task<double> UpdateBattery(ButtplugClientDevice device)
        {
            try
            {
                return await device.BatteryAsync();
            }
            catch (HttpRequestException e)
            {
                Plugin.Log.Info("Lovense Connect not reachable.");
            }
            return double.NaN;
        }

        public async Task TestDevice(ButtplugClientDevice device)
        {
            await VibrateToy(device, 1000, 0.5);
        }

        public async Task VibrateToy(ButtplugClientDevice device, int delay = 0, double speed = 0.5)
        {
            await device.VibrateAsync(speed);
            if (delay > 0)
            {
                await Task.Delay(delay);
                await device.VibrateAsync(0);
            }
        }

        public async Task PresetToy(ButtplugClientDevice device, int time, int preset = 2, bool resume = false)
        {
            await VibratePresetToy(device, preset);
            //TODO: Voir presets
            //if (resume)
            //{
            //    await Task.Delay(4000);
            //    toy.resume();
            //}
        }

        public async Task VibratePresetToy(ButtplugClientDevice device, int preset = 2)
        {
            //Todo: Presets
            await StartPresetToy(device, preset);
            await Task.Delay(2000);
            await StopToy(device);
        }

        public async Task StartToy(ButtplugClientDevice device, double speed)
        {
            await device.VibrateAsync(speed);
            //Todo:Handle rotate, air, etc

        }
        public async Task StartPresetToy(ButtplugClientDevice device, int preset = 0)
        {
            //Todo: Handle presets
        }

        public async Task StopToy(ButtplugClientDevice device)
        {
            await device.Stop();
        }
    }
}
