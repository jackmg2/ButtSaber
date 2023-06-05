using System;
using System.Threading;
using System.Threading.Tasks;
using Buttplug.Client;
using ButtSaber.Configuration;
using ModestTree;

namespace ButtSaber.Classes
{
    public class Toy
    {
        public ButtplugClientDevice Device { get; private set; }

        private bool _connected;
        private bool _isRunning;
        private ToysConfig _config;
        private double _lastLevel;
        private String _lastConnection;
        private static int _vibrationOrdersInQueue = 0;
        private static int _10HZCallCounter = 0;
        private static Timer _10HzLimitTimer;

        public Toy(ButtplugClientDevice device, bool Connected = false)
        {
            _10HzLimitTimer = new Timer(state => ResetCallCount(), null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            this.Device = device;

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
            this.VibrateAsync(1000, 0.2);
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

        public Task VibrateAsync(int time, double level, bool ignoreLastLevel = false)
        {
            this._isRunning = true;

            if (!ignoreLastLevel)
            {
                this._lastLevel = level;
            }
            Plugin.Log.Debug($"Vibrate at {level} during {time}, ignoreLastLevel: {ignoreLastLevel}");
            return VibrateInternal(time, level);
        }



        public Task VibrateAsync(int time, bool hit = true)
        {
            this._isRunning = true;
            this._lastLevel = this.getIntense(hit);
            Plugin.Log.Debug($"Vibrate, _lastLevel: {this._lastLevel} during {time}, hit: {hit}");
            return VibrateInternal(time, this._lastLevel);
        }

        public void resume()
        {
            VibrateInternal(0, this._lastLevel).ConfigureAwait(true);
        }

        private double getIntense(bool hit = true)
        {
            Plugin.Log.Debug($"getIntense, PluginConfig.Instance.IntenseHit: {PluginConfig.Instance.IntenseHit}, PluginConfig.Instance.IntenseMiss: {PluginConfig.Instance.IntenseMiss}");
            double intense = (hit ? PluginConfig.Instance.IntenseHit : PluginConfig.Instance.IntenseMiss) / 20;
            Plugin.Log.Debug($"getIntense, hit: {hit}, intense: {intense}");
            if (PluginConfig.Instance.RandomIntenseHit)
            {
                Random rng = new Random();
                intense = (double)rng.Next(1, 20) / 20;
                Plugin.Log.Debug($"getIntense, hit: {hit}, randomIntense: {intense}");
            }
            return intense;
        }

        public void vibratePreset(int preset = 2, bool resume = false)
        {
            this._isRunning = true;
            PresetToy(preset).ConfigureAwait(true);
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
            StopToy().ConfigureAwait(true);
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

        public async Task PresetToy(int time, int preset = 2, bool resume = false)
        {
            await VibratePresetToy(preset);
            //TODO: Voir presets
            //if (resume)
            //{
            //    await Task.Delay(4000);
            //    toy.resume();
            //}
        }

        public async Task VibratePresetToy(int preset = 2)
        {
            //Todo: Presets
            await StartPresetToy(preset);
            await Task.Delay(2000);
            await StopToy();
        }

        public async Task StartPresetToy(int preset = 0)
        {
            //Todo: Handle presets
        }

        private async Task VibrateInternal(int delay = 0, double speed = 0.5)
        {
            Interlocked.Increment(ref _10HZCallCounter);

            // Check if the call count has reached the limit
            if (Interlocked.Increment(ref _10HZCallCounter) > 10)
            {
                Plugin.Log.Debug($"VibrateInternal, 10 calls already done");
                Interlocked.Decrement(ref _10HZCallCounter);
            }
            else
            {

                //Increment the number of order in queue
                Interlocked.Increment(ref _vibrationOrdersInQueue);

                Plugin.Log.Debug($"VibrateInternal, delay: {delay}, speed: {speed}");
                try
                {
                    await this.Device.VibrateAsync(speed);
                }
                catch (Exception e)
                {
                    Plugin.Log.Error($"VibrateInternal, error: {e}");
                }

                if (delay > 0)
                {
                    Timer timer = null;
                    timer = new Timer(async state =>
                    {
                        // Decrement the task counter
                        if (Interlocked.Decrement(ref _vibrationOrdersInQueue) <= 0)
                        {
                            Plugin.Log.Error($"VibrateInternal,Stop Vibrator");
                            try
                            {
                                await this.Device.VibrateAsync(0);
                            }
                            catch (Exception e)
                            {
                                Plugin.Log.Error($"VibrateInternal,Stopping Vibrator failed, error: {e}");
                            }
                        }

                        // Dispose the timer
                        timer.Dispose();
                    }, null, TimeSpan.FromMilliseconds(delay), TimeSpan.Zero);
                }
            }
        }

        private static void ResetCallCount()
        {
            Interlocked.Exchange(ref _10HZCallCounter, 0);
        }

        public async Task StopToy()
        {
            await this.Device.Stop();
        }

    }
}
