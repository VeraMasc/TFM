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
    /// Efecto que añade un modificador
    /// </summary>
    [Serializable]
    public class AddModifier : Targeted, IValueEffect
    {
        /// <summary>
        /// Lista de modificadores a añadir
        /// </summary>
        [SerializeReference,SubclassSelector]
        public List<BaseModifier> modifiers;


        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Card card){
                foreach(var mod in modifiers){
                    CardModifiers.addModifier(card,mod);
                }
                
            }
            else{
                Debug.LogError("Modifiers can only be added to cards");
            }
            yield break;
        }

    }
}
