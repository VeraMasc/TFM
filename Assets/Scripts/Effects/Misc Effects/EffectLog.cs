using System;
using System.Collections;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;

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
            Debug.Log(message.getValueObj(context), (UnityEngine.Object)context.self);
            yield break;
        }


    }
}
