using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Effect
{
    /// <summary>
    /// Gestiona los targets que tienen varios subtargets
    /// </summary>
    [Serializable]
    public abstract class CompositeTargeter:EffectTargeter
    {
        [SerializeReference,SubclassSelector]
        public List<EffectTargeter> subtargeters;

        
        public override List<EffectTargeter> getTargeterNodes(){
            var ret = new List<EffectTargeter>(){this};
            foreach(var subtargeter in subtargeters){
                ret.AddRange(subtargeter.getTargeterNodes());
            }
            return ret;
        }

    }

}

