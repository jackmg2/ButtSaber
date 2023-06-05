
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ButtSaber.Configuration;
using ButtSaber.Classes.Modus;
using System.Linq;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;

namespace ButtSaber.Classes
{
    class Control
    {
        private ButtplugClient _client = new ButtplugClient("Butt Saber");
        
        private List<Toy> Toys = new List<Toy>();

        private DefaultModus ActiveMode = new DefaultModus();

        public List<object> AvailableModi = new object[] { "Default" }.ToList();

        private Dictionary<string, DefaultModus> ModiList = new Dictionary<string, DefaultModus> {
            { "Default", new DefaultModus() }
        };

        public int HitCounter = 0;
        public int MissCounter = 0;
        
        public Control()
        {
            this.LoadModes();
            this.SetMode();
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

        public void SetMode()
        {
            if (ModiList.ContainsKey(PluginConfig.Instance.Modus))
            {
                this.ActiveMode = ModiList[PluginConfig.Instance.Modus];
            }
            else
            {
                this.ActiveMode = new DefaultModus();
            }
        }

        public DefaultModus GetMode()
        {
            return this.ActiveMode;
        }

        public async Task ConnectAsync()
        {
            await this.Connect();
            Toys = GetToyList().ToList();
        }

        public IEnumerable<Toy> GetToyList()
        {
            return _client.Devices.Select(d => new Toy(d, true));
        }

        public void HandleCut(bool LHand, bool success, NoteCutInfo data = new NoteCutInfo())
        {
            if (success)
            {
                Plugin.Control.HitCounter++;
                Plugin.Log.Debug($"HandleCut, HitCounter: {Plugin.Control.HitCounter}, LHand: {LHand}, success: {success}, data: {data}");
                this.ActiveMode.HandleHit(Toys, LHand, data);
            }
            else
            {
                Plugin.Control.MissCounter++;
                Plugin.Log.Debug($"HandleCut, MissCounter: {Plugin.Control.MissCounter}, LHand: {LHand}, success: {success}, data: {data}");
                this.ActiveMode.HandleMiss(Toys, LHand);
            }
        }

        public void HandleBomb()
        {
            this.ActiveMode.HandleBomb(this.Toys);
        }

        public void HandleFireworks()
        {
            this.ActiveMode.HandleFireworks(this.Toys);
        }

        public void StopActive()
        {
            foreach (Toy toy in this.Toys)
            {
                if (toy.IsConnected())
                {
                    toy.stop(true);
                }
            }
        }

        public bool IsAToyActive()
        {
            foreach (Toy toy in this.Toys)
            {
                if (toy.IsConnected() && toy.IsActive())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsToyAvailable()
        {
            return this.Toys.Count > 0;
        }

        public void PauseGame()
        {
            foreach (Toy toy in this.Toys)
            {
                if (toy.IsConnected())
                {
                    toy.stop();
                }
            }
        }

        public void PunishmentBreak()
        {
            foreach (Toy toy in this.Toys)
            {
                if (toy.IsConnected())
                {
                    Random rng = new Random();
                    int intense = rng.Next(15, 20);

                    toy.VibrateAsync(0, intense, !this.ActiveMode.useLastLevel());
                }

            }
        }



        public void ResumeGame()
        {
            foreach (Toy toy in this.Toys)
            {
                if (toy.IsConnected())
                {
                    toy.resume();
                }
            }
        }

        public void EndGame()
        {
            this.StopActive();
        }

        public void ResetCounter()
        {
            this.HitCounter = 0;
            this.MissCounter = 0;
        }

        public void LoadModes()
        {
            foreach (string obj in Utilities.GetAllClasses("ButtSaber.Classes.Modus"))
            {
                if (obj.Equals("Modus") || obj.Equals("DefaultModus")) continue;
                Type modi = Type.GetType("ButtSaber.Classes.Modus." + obj);
                if (modi != null)
                {
                    DefaultModus activeObj = Activator.CreateInstance(modi) as DefaultModus;
                    AvailableModi.Add(activeObj.GetModusName());
                    ModiList.Add(activeObj.GetModusName(), activeObj);
                }
            }
        }
    }
}
