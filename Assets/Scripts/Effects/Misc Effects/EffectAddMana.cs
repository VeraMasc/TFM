using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;
using System.Linq;

namespace Effect{
    /// <summary>
    /// Efecto que añade 1 de maná
    /// </summary>
    [Serializable]
    public class AddMana : Targeted, IValueEffect
    {
        
        [SerializeReference, SubclassSelector]
        public IValue pips = new ManaValue();



        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Entity entity ){
                var val = pips.getValueObj(context);
                Debug.Log(val);
                if( val is ICollection<Mana> collection)
                {
                    
                    entity.mana.pips.AddRange(collection);
                    entity.mana.updateDisplay();
                }
                else{
                    Debug.LogError($"Can't add non-mana values ({pips.getValueObj(context).GetType().Name})");
                }
                
            }else{
                Debug.LogError("Only entities can add mana");
            }
            yield break;
        }




    }

    [Serializable]
    public class ManaValue:IValue
    {
        public string mana;

        private ManaCost parsed=null;

        public object getValueObj(Context context)
        {
            if(mana == parsed?.costText){ //Si ya lo había calculado antes
                return parsed?.pips ?? new List<Mana>();
            }

            parsed ??= new ManaCost();
            parsed.costText = mana;
            parsed.parseCost();
            return parsed.pips;
        }
    }
}
