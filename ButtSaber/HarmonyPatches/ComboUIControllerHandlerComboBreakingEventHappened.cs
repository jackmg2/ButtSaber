using HarmonyLib;
using ButtSaber.Configuration;
using UnityEngine;

namespace ButtSaber
{
    [HarmonyPatch(typeof(BombNoteController), "HandleWasCutBySaber")]
    class HandleWasCutBySaber
    {
        static void Prefix(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec)
        {
            
            if (PluginConfig.Instance.Enabled)
            {
                Plugin.Control.HandleBomb();
            }
        }
    }

}
