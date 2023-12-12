﻿using ButtSaber.Configuration;
using System;
using System.Collections.Generic;


namespace ButtSaber.Classes.Modus
{
    class DefaultModus
    {

        public virtual void HandleHit(List<Toy> toys, bool LHand, NoteCutInfo data)
        {
            foreach (Toy toy in toys)
            {
                if (PluginConfig.Instance.VibrateHit)
                {                    
                    if (toy.CheckHand(LHand) && !toy.GetToyConfig().Random)
                    {
                        Plugin.Log.Debug($"DefaultModus.HandleHit, LHand: {LHand}, toy.GetToyConfig().Random: {toy.GetToyConfig().Random}, duration: {PluginConfig.Instance.DurationHit}");
                        toy.VibrateAsync(PluginConfig.Instance.DurationHit);
                    }

                    if (toy.GetToyConfig().Random)
                    {
                        Random rng = new Random();
                        bool random = rng.Next(0, 2) > 0;
                        if ((random && LHand) || (!random && !LHand))
                        {
                            Plugin.Log.Debug($"DefaultModus.HandleHit, LHand: {LHand}, toy.GetToyConfig().Random: {toy.GetToyConfig().Random}, rng: {rng}, duration: {PluginConfig.Instance.DurationHit}");
                            toy.VibrateAsync(PluginConfig.Instance.DurationHit);
                        }
                    }
                }
            }
        }

        public virtual void HandleMiss(List<Toy> toys, bool LHand)
        {
            foreach (Toy toy in toys)
            {
                if (PluginConfig.Instance.VibrateMiss)
                {
                    if (toy.CheckHand(LHand) && !toy.GetToyConfig().Random)
                    {
                        Plugin.Log.Debug($"DefaultModus.HandleHit, LHand: {LHand}, toy.GetToyConfig().Random: {toy.GetToyConfig().Random}, duration: {PluginConfig.Instance.DurationMiss}");
                        toy.VibrateAsync(PluginConfig.Instance.DurationMiss, false);
                    }

                    if (toy.GetToyConfig().Random)
                    {
                        Random rng = new Random();
                        bool random = rng.Next(0, 2) > 0;
                        if ((random && LHand) || (!random && !LHand))
                        {
                            Plugin.Log.Debug($"DefaultModus.HandleHit, LHand: {LHand}, toy.GetToyConfig().Random: {toy.GetToyConfig().Random}, rng: {rng}, duration: {PluginConfig.Instance.DurationMiss}");
                            toy.VibrateAsync(PluginConfig.Instance.DurationMiss, false);
                        }
                    }
                }
            }
        }

        public virtual void HandleBomb(List<Toy> toys)
        {
            if (!PluginConfig.Instance.VibeBombs) return;

            foreach (Toy toy in toys)
            {
                toy.vibratePreset(PluginConfig.Instance.PresetBomb);
            }
        }

        public virtual void HandleFireworks(List<Toy> toys)
        {
            if (!PluginConfig.Instance.Fireworks) return;

            foreach (Toy toy in toys)
            {
                toy.vibratePreset(PluginConfig.Instance.PresetBomb);
            }
        }

        public virtual string GetModusName()
        {
            return "Default";
        }

        public virtual List<string> GetUiElements()
        {
            return new List<string> { "vibrateOnMissBtn", "vibrateOnHitBtn", "randomIntenseMissBtn", "intenseMissSlider", "durationMissSlider", "randomIntenseHitBtn", "intenseHitSlider", "durationHitSlider", "presetOnBombHit", "presetBombSlider", "fireworksBtn" };
        }

        public virtual string GetDescription()
        {
            return "Default modus, free configuration for hit and miss boxes, also bombs behavior and fireworks.";
        }

        public virtual bool UseLastLevel()
        {
            return false;
        }


    }
}
