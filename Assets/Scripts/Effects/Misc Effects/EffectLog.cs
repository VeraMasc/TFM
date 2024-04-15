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
    /// Efecto que Printea un log
    /// </summary>
    [Serializable]
    public class EffectLog : EffectScript, IValueEffect
    {
        /// <summary>
        /// Mensaje a mostrar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public IValue message;


        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            var raw = message.getValueObj(context);
            var mapped = displayMapper(raw);
            Debug.Log(mapped, (UnityEngine.Object)context.self);
            yield break;
        }

        public string displayMapper(object raw, int depth=0){
            if(depth>10)
                return "*MAX DEPTH REACHED*"; //Evitar recursión infinita

            if(raw is IEnumerable<object> collection){
                var ret = String.Join(", ",collection.Select(obj => displayMapper(obj, depth+1)));
                return "["+ret+"]";
            }
            if(raw is ITargetable targetable){
                return (targetable as Component).gameObject.name;
            }

            return raw.ToString();
        }


    }
}
