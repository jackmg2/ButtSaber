using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System.Linq;
using ButtSaber.Configuration;

namespace ButtSaber.Classes
{
    public class Request
    {
        private ButtplugClient _client = new ButtplugClient("Butt Saber");

        public Request()
        {
        }

        public async Task Connect()
        {
            string url = PluginConfig.Instance.GetActiveConnections().First().Value.CreateBaseUrl();
            Plugin.Log.Debug("Request.Connect: " + url);
            try
            {
                await _client.ConnectAsync(new ButtplugWebsocketConnector(new Uri(url)));
                Plugin.Log.Debug("Connected.");
            }
            catch (Exception e)
            {
                Plugin.Log.Error("Request.Connect: " + e.Message);
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

        

        
    }
}
