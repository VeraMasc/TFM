using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Effect{
    /// <summary>
    /// Permite hacer targeters con requisitos especiales para los targets
    /// </summary>
    [Serializable]
    public class ConditionedTargeter:EffectTargeter 
    {
        /// <summary>
        /// Targeter que usa para obtener los candidatos a cumplir las condiciones
        /// </summary>
        [SerializeReference,SubclassSelector]
        public EffectTargeter baseTargeter;

        [SerializeReference,SubclassSelector]
        public List<Condition> conditions = new List<Condition>();

        public override void resolveTarget(Context context){
            baseTargeter.resolveTarget(context);
            //TODO: filter base targeter with conditions
        }
    }
}