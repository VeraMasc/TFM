using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using Effect.Preset;
using UnityEngine.TextCore;

namespace Effect{
    /// <summary>
    /// Efecto que revela cartas
    /// </summary>
    [Serializable]
    public class RevealCard : Targeted, IValueEffect
    {

        /// <summary>
        /// Hace que el efecto oculte cartas en vez de revelarlas
        /// </summary>
        public bool hideInstead;

        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Card card){
                
                if(!card.isFaceUp() ^ hideInstead){
                    Debug.Log("Reveal");
                    card.SetFacing(hideInstead? CardFacing.FaceDown : CardFacing.FaceUp);
                    
                    yield return new WaitForSeconds(0.75f / card.FlipAnimator.speed);
                }
            }
            else{
                Debug.LogError($"Can't hide/reveal non card object of type {target.GetType()}", (UnityEngine.Object)target);
            }
        }
    }
}