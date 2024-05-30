using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;
using System.Linq;
using Common.Coroutines;

namespace Effect{
    /// <summary>
    /// Efecto que engloba varios efectos relacionados con entidades
    /// </summary>
    [Serializable]
    public class EntityEffect : Targeted, IValueEffect
    {
        [SerializeReference,SubclassSelector]
        public EntityEffectOption effectOption;

        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Entity entity){
                yield return effectOption.executeOption(entity,context).Start(stack);
            }
            
        }

    }

    public abstract class EntityEffectOption{

        public virtual IEnumerator executeOption(Entity entity, Context context)
        {
            throw new NotImplementedException();
        }
    }
    namespace Option
    {
        [Serializable]
        public class Shuffle:EntityEffectOption{
            public override IEnumerator executeOption(Entity entity, Context context)
            {
                yield return entity.shuffle().Start(entity);
            }
        }
    }
}
