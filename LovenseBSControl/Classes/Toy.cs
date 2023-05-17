using System;
using System.Threading.Tasks;
using Buttplug.Client;
using LovenseBSControl.Configuration;

namespace LovenseBSControl.Classes
{
    public class Toy
    {
        private Request _request;
        public ButtplugClientDevice Device { get; private set; }

        private bool _connected;
        private bool _isRunning;
        private ToysConfig _config;
        private double _lastLevel;
        private String _lastConnection;

        public Toy(ButtplugClientDevice device, Request request, bool Connected = false)
        {
            this.Device = device;
            this._request = request;

            ToysConfig newConfig = ToysConfig.createToyConfig(this.Device.Name);
            if (PluginConfig.Instance.ToyConfigurations != null)
            {
                if (PluginConfig.Instance.ToyConfigurations != null && PluginConfig.Instance.IsAdded(this.Device.Name))
                {
                    newConfig = PluginConfig.Instance.getToyConfig(this.Device.Name);
                }
                else
                {
                    PluginConfig.Instance.AddToyConfiguration(this.Device.Name, newConfig);
                }
            }
            _config = newConfig;
            this._connected = Connected;
            this._isRunning = false;
        }

        public string GetPictureName()
        {
            //Todo: Display device pictures
            return ("logo_machine.png").ToLower();
        }

        public bool IsConnected()
        {
            //TODO: fix
            return true;
        }

        public bool IsActive()
        {
            //TODO: fix
            return true;
        }

        public bool IsOn()
        {
            return this._isRunning;
        }

        public bool CanRotate()
        {
            //Todo: Handle can rotate
            return false;
        }

        public bool CanPump()
        {
            //Todo: Handle can pump
            return false;
        }

        public void Test()
        {
            this.vibrate(1000, 0.5);
        }

        public void setOff()
        {
            this._isRunning = false;
        }
        internal void setOn()
        {
            this._isRunning = true;
        }

        public bool CheckHand(bool LHand)
        {
            return (LHand == this._config.LHand) || (!LHand == this._config.RHand);
        }

        public void vibrate(int time, double level, bool ignoreLastLevel = false)
        {
            this._isRunning = true;

            if (!ignoreLastLevel)
            {
                this._lastLevel = level;
            }
            _request.VibrateToy(this.Device, time, level).ConfigureAwait(true);
        }

        public void vibrate(int time, bool hit = true)
        {
            this._isRunning = true;
            this._lastLevel = this.getIntense(hit);
            _request.VibrateToy(this.Device, time, this._lastLevel).ConfigureAwait(true);
        }

        public void resume()
        {
            _request.VibrateToy(this.Device, 0, this._lastLevel).ConfigureAwait(true);
        }

        private double getIntense(bool hit = true)
        {
            double intense = (hit ? PluginConfig.Instance.IntenseHit : PluginConfig.Instance.IntenseMiss) / 20;
            if (PluginConfig.Instance.RandomIntenseHit)
            {
                Random rng = new Random();
                intense = (double)rng.Next(1, 20) / 20;
            }
            return intense;
        }

        public void vibratePreset(int preset = 2, bool resume = false)
        {
            this._isRunning = true;
            _request.PresetToy(this.Device, preset).ConfigureAwait(true);
        }

        public async Task<String> GetBattery()
        {
            return (await this.Device.BatteryAsync()).ToString();
        }

        public void stop(bool resetLastLevel = false)
        {
            if (resetLastLevel)
            {
                this._lastLevel = 0;
            }
            this._isRunning = false;
            _request.StopToy(this.Device).ConfigureAwait(true);
        }

        public ToysConfig GetToyConfig()
        {
            return _config;
        }

        public void SetConnection(string connectionName)
        {
            this._lastConnection = connectionName;
        }

        public string GetConnection()
        {
            return this._lastConnection;
        }

    }
}
