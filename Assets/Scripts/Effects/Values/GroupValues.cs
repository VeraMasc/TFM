
using System;
using CardHouse;

namespace Effect.Value{

    /// <summary>
    /// Obtiene la cantidad de cartas que hay en un grupo
    /// </summary>
    [Serializable]
    public class CardsInGroup:NumericCheck{
        //TODO: Add value<group> parameter

        public override int getValue(Context context)
        {
            return ((Card) context?.self)?.Group.MountedCards.Count ?? 0; 
        }
    }
}
