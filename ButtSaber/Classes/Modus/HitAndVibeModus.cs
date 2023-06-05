using ButtSaber.Configuration;
using System.Collections.Generic;

namespace ButtSaber.Classes.Modus
{
    class HitAndVibeModus : Modus
    {
        private int _numberOfMisses = 0;
        public override void HandleHit(List<Toy> toys, bool LHand, NoteCutInfo data)
        {
            foreach (Toy toy in toys)
            {
                if (PluginConfig.Instance.VibrateHit)
                {
                    if (toy.CheckHand(LHand))
                    {
                        double intensity = 0.1;
                        if(_numberOfMisses > 20)
                        {
                            intensity = 1;
                        }
                        else
                        {
                            intensity = 0.05 * _numberOfMisses;
                        }
                        toy.VibrateAsync(200, intensity);
                    }
                }
            }
        }

        public override void HandleMiss(List<Toy> toys, bool LHand)
        {
            foreach (Toy toy in toys)
            {
                if (PluginConfig.Instance.VibrateMiss)
                {
                    toy.VibrateAsync(200,1);
                    _numberOfMisses++;
                }
            }
        }

        public override void HandleBomb(List<Toy> toys)
        {
            if (!PluginConfig.Instance.VibeBombs) return;

            foreach (Toy toy in toys)
            {
                toy.vibratePreset(PluginConfig.Instance.PresetBomb);
            }
        }

        public override void HandleFireworks(List<Toy> toys)
        {
            if (!PluginConfig.Instance.Fireworks) return;

            foreach (Toy toy in toys)
            {
                toy.vibratePreset(PluginConfig.Instance.PresetBomb);
            }
        }

        public override string GetModusName()
        {
            return "HitAndVibe";
        }

        public override List<string> getUiElements()
        {
            return new List<string> { "vibrateOnMissBtn", "vibrateOnHitBtn", "randomIntenseMissBtn", "intenseMissSlider", "durationMissSlider", "randomIntenseHitBtn", "intenseHitSlider", "durationHitSlider", "presetOnBombHit", "presetBombSlider", "fireworksBtn" };
        }

        public override string getDescription()
        {
            return "Each miss makes it stronger.";
        }

        public override bool useLastLevel()
        {
            return false;
        }
    }
}
