using System;
using System.Collections;
using System.Collections.Generic;
using Common.Coroutines;
using Effect;
using Effect.Status;
using UnityEngine;


namespace Effect
{


    [Serializable]
    /// <summary>
    /// Permite seleccionar entre varios modos
    /// </summary>
    public class ModalEffect : EffectScript, IValueEffect, IPrecalculable
    {
        /// <summary>
        /// Lista de modos posibles
        /// </summary>
        public List<CardMode> modes;

        /// <summary>
        /// Modo escogido
        /// </summary>
        [NonSerialized]
        public int chosen;
        public IEnumerator precalculate(CardResolveOperator stack, Context context)
        {
            //TODO: implement mode selection
            chosen =0;
            yield break;
        }

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            if(modes.Count > chosen){ //Chosen is within bounds
                foreach(var effect in modes[chosen].effects){
                   yield return UCoroutine.Yield(effect.execute(stack,context));
                }
                
            }
        }
    }

    [Serializable]
    public class CardMode{
        [TextArea(3, 10)]
        public string text;
        [SerializeReference, SubclassSelector]
        public List<EffectScript> effects;
        
    }
}