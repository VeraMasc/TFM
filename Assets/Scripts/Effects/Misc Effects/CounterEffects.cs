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

namespace Effect{
    /// <summary>
    /// Efecto que interactua con los contadores
    /// </summary>
    [Serializable]
    public class SetCounters : Targeted, IValueEffect
    {
        [SerializeReference, SubclassSelector]
        public Mode mode = new Add();


        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            mode?.implement(target, context);
            yield break;
        }
        public abstract class Mode{
            public virtual void implement(ITargetable target,Context context){
                throw new NotImplementedException();
            }
        }

        [Serializable]
        public class Add:Mode
        {
            [SerializeReference,SubclassSelector]
            public IValue counterType = new Value.TextString();

            [SerializeReference,SubclassSelector]
            public IValue amount = new Value.Numeric(1);

            public override void implement(ITargetable target,Context context){
                var type = counterType.getValueObj(context) as string;
                var num = amount.getValueObj(context) as int?;
                Debug.Log($"{context.precalculated}  {num}");
                if(!context.precalculated && num<0){
                    var current = CounterHolder.getCounter(target,type);
                   
                    if((current+num)<0){
                         Debug.Log($"{current-num}");
                        context.mode = ExecutionMode.cancel;
                        return;
                    }
                    
                }
                CounterHolder.addCounter(target, type, num??0);
                
                
            }
        }

        [Serializable]
        public class Set:Add
        {
            public override void implement(ITargetable target,Context context){
                var type = counterType.getValueObj(context) as string;
                var num = amount.getValueObj(context) as int?;

                CounterHolder.setCounter(target, type, num??0);
            }
        }
        public class Clear:Mode
        {
            public override void implement(ITargetable target,Context context){
                var holder = CounterHolder.getHolder(target);
                if(holder != null){
                    holder.clear();
                    GameObject.Destroy(holder);
                }
            }
        } 
    }
    
    
    
}