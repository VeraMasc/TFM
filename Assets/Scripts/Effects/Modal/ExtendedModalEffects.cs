using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using DTT.Utils.Extensions;
using Effect;
using Effect.Status;
using UnityEngine;


namespace Effect
{


    [Serializable]
    /// <summary>
    /// Modal cuyos modos se escogen al azar
    /// </summary>
    public class RandomModal : ModalEffect
    {
        
        public override IEnumerator precalculate(CardResolveOperator stack, Context context)
        {
            var modeSettings = modes.Select(m => new ModalOptionSettings(){tag=m.id}).ToArray();
            chosen = modeSettings.Select((m,index)=> index)
                .TakeRandom(maxChoices)
                .ToList();

            //Precalculate chosen modes
            if(chosen?.Any()??false){
                int index=0;
                foreach(var mode in modes){
                    if(chosen.Contains(index)){
                        context.choiceTreeDeepen();
                        yield return UCoroutine.Yield(Precalculate.precalculateEffects(mode.effects,context));
                        context.choiceTreePop();
                    }
                    
                    index++;
                }
    
                //Change text temporarily
                modalChangeText(chosen.Select(i => modes[i].id).ToList(), context);
            }
            context.choiceTreeIncrease();
            
        }

       

        
    }

       

    
}