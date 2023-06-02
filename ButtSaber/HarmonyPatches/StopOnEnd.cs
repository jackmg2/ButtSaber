using HarmonyLib;
using ButtSaber.Configuration;

namespace ButtSaber.HarmonyPatches
{

    [HarmonyPatch(typeof(ScoreController), "OnDestroy")]
    class OnDestroy
    {
        static void Prefix()
        {
            if (PluginConfig.Instance.Enabled)
            {
                Plugin.Control.EndGame();
            }
        }
    }
}

