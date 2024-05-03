using System;
using System.Collections;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class EnterBoard : EffectScript
    {
        
 

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            
            context.resolutionPile = CombatController.singleton.board;
            yield break;
            
        }

       

    }
}
