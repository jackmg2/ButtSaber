using ButtSaber.Configuration;
using System;
using System.Collections.Generic;

namespace ButtSaber.Classes.Modus
{
    class Challenge1Modus : Modus
    {

        public override void HandleHit(List<Toy> toys, bool LHand, NoteCutInfo data)
        {
            Plugin.Log.Debug($"HandleHit, Plugin.Control.HitCounter: {Plugin.Control.HitCounter}, LHand: {LHand}, data: {data}");
            if (Plugin.Control.HitCounter >= 15)
            {
                Plugin.Log.Debug($"HandleHit, Plugin.Control.MissCounter: {Plugin.Control.MissCounter}");
                Plugin.Control.MissCounter = Math.Max(--Plugin.Control.MissCounter, 0);
                Plugin.Log.Debug($"HandleHit, Plugin.Control.MissCounter: {Plugin.Control.MissCounter}");
                Plugin.Control.HitCounter = 0;
                foreach (Toy toy in toys)
                {
                    if (toy.IsActive())
                    {
                        var intensity = (double)Plugin.Control.MissCounter / 20;
                        Plugin.Log.Debug($"HandleHit, intensity: {intensity}");
                        toy.VibrateAsync(0, intensity);
                    }
                }
            }
            this.HandleMiss(toys, LHand);
        }

        public override void HandleMiss(List<Toy> toys, bool LHand)
        {
            Plugin.Log.Debug($"HandleMiss, Plugin.Control.HitCounter: {Plugin.Control.HitCounter}, LHand: {LHand}");
            Plugin.Control.HitCounter = 0;

            Plugin.Log.Debug($"HandleMiss, Plugin.Control.MissCounter: {Plugin.Control.MissCounter}");
            Plugin.Control.MissCounter = Math.Min(Plugin.Control.MissCounter, 20);
            Plugin.Log.Debug($"HandleMiss, Plugin.Control.MissCounter: {Plugin.Control.MissCounter}");

            foreach (Toy toy in toys)
            {
                if (toy.IsActive())
                {
                    var intensity = (double)Plugin.Control.MissCounter / 20;
                    Plugin.Log.Debug($"HandleMiss, intensity: {intensity}");
                    toy.VibrateAsync(0, intensity);
                }
            }
        }

        public override void HandleBomb(List<Toy> toys)
        {
            if (!PluginConfig.Instance.VibeBombs) return;

            foreach (Toy toy in toys)
            {
                if (toy.IsActive())
                {
                    toy.vibratePreset(3, true);
                }
            }
        }
        public override void HandleFireworks(List<Toy> toys)
        {

        }

        public override string GetModusName()
        {
            return "Challenge 1";
        }

        public override List<string> GetUiElements()
        {
            return new List<string> { "fireworksBtn", "presetOnBombHit", "presetBombSlider" };
        }

        public override string GetDescription()
        {
            return "Vibrate missing boxes. Each time level go up, after 15 hits, level goes one step down";
        }

        public override bool UseLastLevel()
        {
            return true;
        }

    }
}
