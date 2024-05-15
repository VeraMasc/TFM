using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public int maxChoices=1;
        /// <summary>
        /// Lista de modos posibles
        /// </summary>
        public List<CardMode> modes;

        /// <summary>
        /// Modos escogidos
        /// </summary>
        [NonSerialized]
        public List<int> chosen=new();
        public IEnumerator precalculate(CardResolveOperator stack, Context context)
        {
            var modeSettings = modes.Select(m => new ModalOptionSettings(){tag=m.id}).ToArray();
            yield return UCoroutine.Yield(GameUI.singleton.getInput(GameUI.singleton.prefabs.cardSelectInput, 
                obj => {chosen = (List<int>)obj;},
                new InputParameters{ values= (object[])modeSettings, context=context,
                    extraConfig = new CardSelectInput.ExtraInputOptions(){maxChoices=maxChoices}
                }));

            //Precalculate chosen modes
            int index=0;
            foreach(var mode in modes){
                if(chosen.Contains(index)){
                    yield return UCoroutine.Yield(Precalculate.precalculateEffects(mode.effects,context));
                }
                index++;
            }
        }

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            int index=0;
            foreach(var mode in modes){
                if(chosen.Contains(index)){
                    foreach(var effect in mode.effects){
                        yield return UCoroutine.Yield(effect.execute(stack,context));
                    }
                }
                index++;
            }
            
        }
    }

    [Serializable]
    public class CardMode{
        public string id;
        [SerializeReference, SubclassSelector]
        public List<EffectScript> effects;
        
    }
}