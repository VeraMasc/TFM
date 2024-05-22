using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Effect.Condition{
    /// <summary>
    /// Requiere que sea el turno de una entidad concreta
    /// </summary>
    [Serializable]
    public class TurnCondition:BaseCondition
    {
        [SerializeReference,SubclassSelector]
        public EffectTargeter turnOwner;

        public override bool check(object inputs, Context context){
            var targets = turnOwner.getTargets(context);
            if(CombatController.singleton && targets!=null){
                return targets.Contains(CombatController.singleton.currentTurn);
            }
            return false;
        }
    }
}