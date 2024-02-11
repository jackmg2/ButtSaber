﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using System.Collections.Generic;
using ButtSaber.Configuration;
using ButtSaber.Classes;
using HMUI;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using BeatSaberMarkupLanguage.Components.Settings;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;
using BeatSaberMarkupLanguage.ViewControllers;
using IPA.Utilities;

namespace ButtSaber.UI
{
    [HotReload(RelativePathToLayout = @"Views\SettingsView.bsml")]
    [ViewDefinition("ButtSaber.UI.Views.SettingsView.bsml")]
    internal class SettingsViewController : BSMLAutomaticViewController
    {
        private Toy selectedToy = null;
        private int selectedToyNumber = 0;
        private ConnectionConfig selectedConnection = null;
        private int selectedConnectionNumber = 0;

        private string connectionName = "New Connection";
        private string ipAdress = "127.0.0.1";
        private string port = "12345";

        private readonly PluginManager pluginManager = new PluginManager();

        private Dictionary<string, GameObject> UiElements = new Dictionary<string, GameObject>();

        [UIValue("enabled")]
        public bool Enabled
        {
            get => PluginConfig.Instance.Enabled;
            set => PluginConfig.Instance.Enabled = value;
        }

        [UIValue("breakPunishment")]
        public bool BreakPunishment
        {
            get => PluginConfig.Instance.BreakPunishment;
            set => PluginConfig.Instance.BreakPunishment = value;
        }

        [UIValue("vibrateMiss")]
        public bool VibrateMiss
        {
            get => PluginConfig.Instance.VibrateMiss;
            set
            {
                randomIntenseMissBtn.gameObject.SetActive(value);
                intenseMissSlider.gameObject.SetActive(value && !PluginConfig.Instance.RandomIntenseMiss);
                durationMissSlider.gameObject.SetActive(value);
                ResetVertical();
                PluginConfig.Instance.VibrateMiss = value;
            }
        }

        [UIValue("vibrateHit")]
        public bool VibrateHit
        {
            get => PluginConfig.Instance.VibrateHit;
            set
            {
                randomIntenseHitBtn.gameObject.SetActive(value);
                intenseHitSlider.gameObject.SetActive(value && !PluginConfig.Instance.RandomIntenseHit);
                durationHitSlider.gameObject.SetActive(value);
                ResetVertical();
                PluginConfig.Instance.VibrateHit = value;
            }
        }

        [UIValue("vibrateBombHit")]
        public bool VibrateBomb
        {
            get => PluginConfig.Instance.VibeBombs;
            set
            {
                presetBombSlider.gameObject.SetActive(value);
                PluginConfig.Instance.VibeBombs = value;
            }
        }

        [UIObject("vibrateOnMissBtn")]
        private GameObject vibrateOnMissBtn;

        [UIObject("vibrateOnHitBtn")]
        private GameObject vibrateOnHitBtn;

        [UIObject("presetOnBombHit")]
        private GameObject presetOnBombHit;

        [UIObject("fireworksBtn")]
        private GameObject fireworksBtn;

        [UIObject("randomIntenseHitBtn")]
        private GameObject randomIntenseHitBtn;

        [UIComponent("modeSelection")]
        private RectTransform modeSelection;

        [UIValue("randomIntenseHit")]
        public bool RandomIntenseHit
        {
            get => PluginConfig.Instance.RandomIntenseHit;
            set
            {
                intenseHitSlider.gameObject.SetActive(!value);
                PluginConfig.Instance.RandomIntenseHit = value;
            }
        }

        [UIObject("randomIntenseMissBtn")]
        private GameObject randomIntenseMissBtn;

        [UIValue("randomIntenseMiss")]
        public bool RandeomIntenseMiss
        {
            get => PluginConfig.Instance.RandomIntenseMiss;
            set
            {
                intenseMissSlider.gameObject.SetActive(!value);
                PluginConfig.Instance.RandomIntenseMiss = value;
            }
        }

        [UIObject("intenseHitSlider")]
        private GameObject intenseHitSlider;

        [UIValue("intenseHit")]
        public int IntenseHit
        {
            get => PluginConfig.Instance.IntenseHit;
            set => PluginConfig.Instance.IntenseHit = value;
        }

        [UIObject("intenseMissSlider")]
        private GameObject intenseMissSlider;

        [UIValue("intenseMiss")]
        public int IntenseMiss
        {
            get => PluginConfig.Instance.IntenseMiss;
            set => PluginConfig.Instance.IntenseMiss = value;
        }

        [UIObject("durationHitSlider")]
        private GameObject durationHitSlider;

        [UIValue("durationHit")]
        public int DurationHit
        {
            get => PluginConfig.Instance.DurationHit;
            set => PluginConfig.Instance.DurationHit = value;
        }

        [UIObject("durationMissSlider")]
        private GameObject durationMissSlider;

        [UIValue("durationMiss")]
        public int DurationMiss
        {
            get => PluginConfig.Instance.DurationMiss;
            set => PluginConfig.Instance.DurationMiss = value;
        }

        [UIObject("vibrationPresetSlider")]
        private GameObject presetBombSlider;

        [UIValue("presetNumber")]
        public int presetNumber
        {
            get => PluginConfig.Instance.PresetBomb;
            set => PluginConfig.Instance.PresetBomb = value;
        }

        [UIValue("vibrateFireworks")]
        public bool VibrateFireworks
        {
            get => PluginConfig.Instance.Fireworks;
            set => PluginConfig.Instance.Fireworks = value;
        }

        [UIComponent("toggleStatusBtn")]
        private TextMeshProUGUI toggleStatusBtn;

        [UIObject("portInput")]
        private GameObject statusPort;

        [UIValue("port")]
        public string Port
        {
            get => this.port;
            set => this.SetProperty(this.port, value);
        }

        [UIObject("ipAdressInput")]
        private GameObject statusIpAdress;

        [UIValue("ipAdress")]
        public string IpAdress
        {
            get => this.ipAdress;
            set => this.SetProperty(this.ipAdress, value);
        }

        [UIValue("connectionName")]
        public string ConnectionName
        {
            get => this.connectionName;
            set => this.SetProperty(this.connectionName, value);
        }

        [UIComponent("connection-list")]
        public CustomListTableData connectionTableData = null;

        [UIComponent("toy-list")]
        public CustomListTableData customListTableData = null;

        private async Task ShowToys(int delay = 0)
        {
            if (delay > 0) await Task.Delay(delay);
            SetupListField();
        }

        [UIAction("clicked-refresh")]
        private async void ClickedReloadAll()
        {
            await ShowToys();
        }

        [UIAction("clicked-test")]
        private void ClickedTest()
        {
            if (this.selectedToy != null && this.selectedToy.IsConnected())
            {
                this.selectedToy.Test();
            }
        }

        [UIAction("clicked-create")]
        private void ClickedCreate()
        {

            PluginConfig.Instance.CreateConnection(this.connectionName, this.ipAdress, this.port).SetStatus(true);

            SetupListField();

        }

        [UIAction("clicked-delete")]
        private void ClickedDelete()
        {
            if (this.selectedConnection != null && !this.selectedConnection.Name.Equals("Default") && !this.selectedConnection.Name.Equals("Localhost"))
            {
                PluginConfig.Instance.DeleteConnection(this.selectedConnection.Name);
                this.selectedConnectionNumber = 0;
                this.selectedConnection = PluginConfig.Instance.GetConnections()[0];
                SetupListField();
            }
        }

        [UIAction("clicked-toggle")]
        private void ClickedToggle()
        {
            if (this.selectedConnection != null)
            {
                this.selectedConnection.SetStatus(!this.selectedConnection.Active);
                SetupListField();
            }
        }

        private string settedSelection = HTypes.bHands;

        [UIValue("listChoice")]
        public string ListChoice
        {
            get
            {
                if (this.selectedToy != null)
                {
                    settedSelection = this.selectedToy.GetToyConfig().HType;
                    return settedSelection;
                }
                return HTypes.bHands;
            }
            set => this.SetProperty(settedSelection, value);
        }

        [UIValue("modusChoice")]
        public string ModusChoice
        {
            get
            {
                return PluginConfig.Instance.Modus;
            }
            set
            {
                PluginConfig.Instance.Modus = value;
                Plugin.Control.SetMode();
                SetUpUIElements();
            }
        }

        [UIComponent("toyConfigSetting")]
        public GenericSetting choice;

        [UIComponent("toyConfigSetting")]
        public DropDownListSetting text;

        [UIAction("onChangeHands")]
        public void onChangeHands(string data)
        {

            if (this.selectedToy != null)
            {
                ToysConfig config = this.selectedToy.GetToyConfig();
                config.setHType(data);
                SetupListField();
            }
        }

        [UIValue("list-options")]
        private List<object> options = new object[] { HTypes.bHands, HTypes.lHand, HTypes.rHand, HTypes.random, HTypes.inactive }.ToList();

        [UIValue("modus-options")]
        private List<object> modi = Plugin.Control.AvailableModi;

        [UIAction("#post-parse")]
        internal void SetupListField()
        {

            SetUpUIElements();
            GetVersion();
            List<ConnectionConfig> Connections = PluginConfig.Instance.GetConnections();
            connectionTableData.data.Clear();

            foreach (ConnectionConfig Connection in Connections)
            {
                Sprite spriteAvail = null;
                if (Connection.Active)
                {
                    spriteAvail = Utilities.LoadSpriteFromResources("ButtSaber.Resources.Sprites.available_profile.png");
                }

                CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(Connection.Name, Connection.Prefix + Connection.IpAdress + ":" + Connection.Port, spriteAvail);
                connectionTableData.data.Add(customCellInfo);
            }
            connectionTableData.tableView.ReloadData();
            this.selectedConnection = Connections[this.selectedConnectionNumber];
            toggleStatusBtn.text = this.selectedConnection.Active ? "Disable" : "Enable";

            connectionTableData.tableView.ScrollToCellWithIdx(this.selectedConnectionNumber, TableView.ScrollPositionType.Beginning, false);
            connectionTableData.tableView.SelectCellWithIdx(this.selectedConnectionNumber);


            if (!Plugin.Control.IsToyAvailable())
            {
                return;
            }

            IEnumerable<Toy> Toys = Plugin.Control.GetToyList();
            customListTableData.data.Clear();
            foreach (Toy toy in Toys)
            {
                Sprite sprite = Utilities.LoadSpriteFromResources("ButtSaber.Resources.Sprites." + toy.GetPictureName());
                ToysConfig toyConfig = toy.GetToyConfig();
                CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(toy.Device.Name, toy.Device.Name + " - " + ((toy.IsConnected() ? "Connected" : "Disconnected") + " - " + toyConfig.HType), sprite);
                customListTableData.data.Add(customCellInfo);
            }

            customListTableData.tableView.ReloadData();
            this.selectedToy = Toys.ElementAt(this.selectedToyNumber);
            customListTableData.tableView.ScrollToCellWithIdx(this.selectedToyNumber, TableView.ScrollPositionType.Beginning, false);
            customListTableData.tableView.SelectCellWithIdx(this.selectedToyNumber);
        }


        [UIAction("toySelect")]
        public void Select(TableView _, int row)
        {
            IEnumerable<Toy> Toys = Plugin.Control.GetToyList();
            this.selectedToyNumber = row;
            this.selectedToy = Toys.ElementAt(row);
            choice.updateOnChange = true;

            choice.associatedValue.SetValue(this.selectedToy.GetToyConfig().HType);
            text.Value = this.selectedToy.GetToyConfig().HType;
        }

        [UIAction("connectionSelect")]
        public void ConnectionSelect(TableView _, int row)
        {
            List<ConnectionConfig> Connections = PluginConfig.Instance.GetConnections();
            this.selectedConnectionNumber = row;
            this.selectedConnection = Connections[row];
            toggleStatusBtn.text = this.selectedConnection.Active ? "Disable" : "Enable";
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            Plugin.Control.SetMode();
        }

        [UIComponent("verticalElement")]
        private LayoutElement verticalElement;

        private void ResetVertical()
        {
            int childCount = 0;
            for (int i = 0; i < verticalElement.transform.childCount; i++)
            {
                if (verticalElement.transform.GetChild(i).gameObject.activeSelf)
                {
                    childCount++;
                }
            }
            verticalElement.minHeight = childCount * 8;
        }

        [UIComponent("detailText")]
        private TextMeshProUGUI detailText;

        private async void GetVersion()
        {
            var release = await pluginManager.GetNewestReleaseAsync();
            if (release != null)
            {
                detailText.text = "ButtSaber " + release.TagName + "\r\n\r\n" + release.Body;
            }

            if (release != null && !release.IsLocalNewest)
            {
                Plugin.Log.Notice(release.TagName);
                //_githubButton.SetActive(true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetUpUIElements()
        {
            UiElements.Clear();
            UiElements.Add("vibrateOnMissBtn", vibrateOnMissBtn);
            UiElements.Add("vibrateOnHitBtn", vibrateOnHitBtn);
            UiElements.Add("randomIntenseMissBtn", randomIntenseMissBtn);
            UiElements.Add("intenseMissSlider", intenseMissSlider);
            UiElements.Add("durationMissSlider", durationMissSlider);
            UiElements.Add("randomIntenseHitBtn", randomIntenseHitBtn);
            UiElements.Add("intenseHitSlider", intenseHitSlider);
            UiElements.Add("durationHitSlider", durationHitSlider);
            UiElements.Add("presetOnBombHit", presetOnBombHit);
            UiElements.Add("presetBombSlider", presetBombSlider);
            UiElements.Add("fireworksBtn", fireworksBtn);

            foreach (var element in UiElements)
            {
                element.Value.SetActive(false);
            }

            foreach (string element in Plugin.Control.GetMode().GetUiElements())
            {
                UiElements[element].SetActive(true);
            }

            if (vibrateOnMissBtn.activeSelf)
            {
                randomIntenseMissBtn.gameObject.SetActive(PluginConfig.Instance.VibrateMiss);
                intenseMissSlider.gameObject.SetActive(PluginConfig.Instance.VibrateMiss && !PluginConfig.Instance.RandomIntenseMiss);
                durationMissSlider.gameObject.SetActive(PluginConfig.Instance.VibrateMiss);
            }

            if (vibrateOnHitBtn.activeSelf)
            {
                randomIntenseHitBtn.gameObject.SetActive(PluginConfig.Instance.VibrateHit);
                intenseHitSlider.gameObject.SetActive(PluginConfig.Instance.VibrateHit && !PluginConfig.Instance.RandomIntenseHit);
                durationHitSlider.gameObject.SetActive(PluginConfig.Instance.VibrateHit);
            }

            if (presetOnBombHit.activeSelf)
            {
                presetBombSlider.gameObject.SetActive(PluginConfig.Instance.VibeBombs);
            }

            ResetVertical();
        }
    }
}
