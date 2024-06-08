using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Effect
{
    /// <summary>
    /// Gestiona los targets que solo filtran otros targets
    /// </summary>
    [Serializable]
    public abstract class TargetFilter:EffectTargeter
    {
        [SerializeReference,SubclassSelector]
        public EffectTargeter targeter;

        //Invierte el output del filtro
        public bool reverseFilter;

        public override void resolveTarget(Effect.Context context){
            var rawTargets = targeter.getTargets(context);
            List<ITargetable> ret = new();
            foreach(var target  in rawTargets){
                if(filterOperation(target) ^ reverseFilter)
                    ret.Add(target);
            }
            _targets = ret.ToArray();
        }

        /// <summary>
        /// Función que sobreescribir para filtrar
        /// </summary>
        /// <param name="targetable"></param>
        /// <returns>True si pasa el filtro</returns>
        public virtual bool filterOperation(ITargetable targetable){
            return true;
        }
    }

    namespace Filter
    {
        /// <summary>
        /// Filtra cartas por tipo
        /// </summary>
        [Serializable]
        public class CardType:TargetFilter
        {
            /// <summary>
            /// Positivo: excluye los triggers. Negativo: solo triggers
            /// </summary>
            public int noTriggers;

            public List<SpeedTypes> speedtypes;
            public List<ActionSubtypes> subtypes;


            public override bool filterOperation(ITargetable targetable){
                if(!(targetable is Card card)){
                    return false;
                }
                    
                var ret=true;
                if (noTriggers!=0){ //Filter triggers
                    ret &= (card.data is TriggerCard)? noTriggers<0: noTriggers>0;
                }
                
                if(card.data is ActionCard action){
                    //Comprueba subtipos
                    var typematch = false;
                    foreach(var subtype in subtypes){
                       typematch |= action.cardType.Contains(subtype.ToString());
                    }
                    ret &=typematch;

                    //Comprueba velocidades
                    var speedMatch = false;
                    foreach(var speed in speedtypes){
                        speedMatch |= action.speedType == speed;
                    }
                    ret &=speedMatch;
                }

                return ret;
            }
        }



        /// <summary>
        /// Filtra cartas de forma aleatoria
        /// </summary>
        [Serializable]
        public class RandomSelection:TargetFilter
        {
            [SerializeReference,SubclassSelector]
            public IValue amount;


            public override void resolveTarget(Effect.Context context){
                var rawTargets = targeter.getTargets(context);

                var val = amount.getValueObj(context);
                if(rawTargets != null && val is int number){
                    _targets = rawTargets.TakeRandom(number).ToArray();
                }
                
            }
        }

    }

}

