using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using ButtSaber.Configuration;
using ButtSaber.UI;
using IPALogger = IPA.Logging.Logger;
using System.Reflection;
using UnityEngine;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Utilities;
using System.Linq;
using UnityEngine.SceneManagement;
using ButtSaber.Classes;

namespace ButtSaber
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Harmony harmony;
        internal static IPALogger Log { get; private set; }

        internal static Classes.Control Control { get; private set; }

        private BeatmapObjectSpawnController SpawnController;

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config conf)
        {
            Log = logger;
            Log.Info("Butt Saber initialized.");

            PluginConfig.Instance = conf.Generated<PluginConfig>();

            CheckConnections();

            Control = new Classes.Control();
            Control.ConnectAsync().ConfigureAwait(true);
        }

        [OnStart]
        public void OnApplicationStart()
        {            
            Log.Debug("OnApplicationStart");
            new GameObject("ButtSaberController").AddComponent<ButtSaberController>();
            BSEvents.gameSceneActive += GameCutAction;
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            harmony = new Harmony("com.jackmg2.BeatSaber.ButtSaber");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            BSMLSettings.instance.AddSettingsMenu("Butt Saber", "ButtSaber.UI.Views.SettingsView.bsml", new SettingsViewController());            
        }

        void GameCutAction()
        {
            if (PluginConfig.Instance.Enabled) ButtSaberController.Instance.GetControllers();
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "GameCore")
            {
                if (SpawnController == null)
                    SpawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
                if (SpawnController == null) return;
            }

        }


        [OnExit]
        public void OnApplicationQuit()
        {
            Control.StopActive();
            
            harmony.UnpatchSelf();
            BSMLSettings.instance.RemoveSettingsMenu("Butt Saber");
            BSEvents.gameSceneActive -= GameCutAction;
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            Log.Debug("OnApplicationQuit");

        }

        private void CheckConnections()
        {
            if (!PluginConfig.Instance.ConnectionExist("Localhost"))
            {
                PluginConfig.Instance.AddConnectionConfiguration( ConnectionConfig.CreatLocalHostConnection());
            }
        }
    }
}
