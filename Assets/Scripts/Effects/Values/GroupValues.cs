
using System;
using System.Collections;
using CardHouse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    /// <summary>
    /// Cuenta la cantidad de valores que tiene una colección
    /// </summary>
    [Serializable]
    public class Count:NumericCheck{
        /// <summary>
        /// Valor a contar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public IValue input;

        //TODO: Add value<group> parameter

        public override int getValue(Context context)
        {
            var value = input.getValueObj(context);
            if(value == null)
                return 0;

            if(value is IEnumerable<object> collection){
                return collection.Count();
            }
            return 1;
        }
    }
}
