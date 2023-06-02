using HarmonyLib;
using ButtSaber.Configuration;

namespace ButtSaber.HarmonyPatches
{


    [HarmonyPatch(typeof(ObstacleSaberSparkleEffectManager), "Start")]
    class BurnMarkPosForSaberType
    {
        static void Prefix()
        {
            if (PluginConfig.Instance.Enabled)
            {
                //TODO
            }
        }
    }
}
