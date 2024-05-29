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
        
        public List<Mana> pips;



        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Entity entity){
                entity.mana.pips.AddRange(pips);
                entity.mana.updateDisplay();
            }else{
                Debug.LogError("Only entities can add mana");
            }
            yield break;
        }




    }
}
