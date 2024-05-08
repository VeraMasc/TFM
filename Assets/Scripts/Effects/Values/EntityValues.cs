
using System;

namespace Effect.Value.Entities{
    [Serializable]
    public class CardsInHand:NumericCheck{
        //TODO: Add value<entity> parameter

        public override int getValue(Context context)
        {
            return context?.controller?.hand?.MountedCards.Count ?? 0; 
        }
    }
}
