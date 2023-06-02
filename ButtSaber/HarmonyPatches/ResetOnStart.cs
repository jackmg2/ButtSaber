using HarmonyLib;
using ButtSaber.Configuration;

namespace ButtSaber.HarmonyPatches
{
    [HarmonyPatch(typeof(ScoreController), "Start")]
    class Start
    {
        static void Prefix()
        {
            if (PluginConfig.Instance.Enabled)
            {
                Plugin.Control.ResetCounter();
            }
        }
    }
}
