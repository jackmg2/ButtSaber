using System.Collections.Generic;

namespace ButtSaber.Classes.Modus
{
    abstract class Modus : DefaultModus
    {
        
        public abstract override void HandleHit(List<Toy> toys, bool LHand, NoteCutInfo data);
        public abstract override void HandleMiss(List<Toy> toys, bool LHand);
        public abstract override void HandleBomb(List<Toy> toys);
        public abstract override void HandleFireworks(List<Toy> toys);

        public abstract override string GetModusName();

        public abstract override List<string> GetUiElements();

        public abstract override string GetDescription();

        public abstract override bool UseLastLevel();

    }
}
