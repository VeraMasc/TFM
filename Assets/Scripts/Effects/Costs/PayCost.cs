using System;
using UnityEngine;


namespace Effect{
    [Serializable]
    public class PayCost:BaseCondition
    {
        [SerializeReference,SubclassSelector]
        public ICost cost;

        /// <summary>
        /// Comprueba si se cumple la condición
        /// </summary>
        public override bool check(Effect.Context context){
            return cost.canBePaid(context);
        }
    }
}