using HarmonyLib;
using ButtSaber.Configuration;

namespace ButtSaber.HarmonyPatches
{
    [HarmonyPatch(typeof(FireworkItemController), "Fire")]
    class Fire
    {
        static void Prefix()
        {
            if (PluginConfig.Instance.Enabled )
            {
                Plugin.Control.HandleFireworks();
            }
        }
    }
}
