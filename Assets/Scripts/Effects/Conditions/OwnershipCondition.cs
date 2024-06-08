using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using CardHouse;
using System.Text.RegularExpressions;


namespace Effect.Condition{
    [Serializable]
    public class OwnershipCondition:BaseCondition
    {

        /// <summary>
        /// Quién recibe el efecto
        /// </summary>
        [SerializeReference, SubclassSelector]
        public EffectTargeter entities;


        public EntityRelation relation;
        /// <summary>
        /// Comprueba si se cumple la condición
        /// </summary>
        public override bool check(object inputs, Context context){

            if(inputs is IEnumerable<object> collection){
                var values = entities.getTargets(context).OfType<Entity>().ToList();
                Debug.Log(String.Join(" ",values));
                return collection.OfType<Card>()
                .All( c => {
                    Debug.Log(c.ownership);
                    if(relation == EntityRelation.controls){
                        return values.Contains(c.ownership.controller);
                    }
                    if(relation == EntityRelation.owns){
                        return values.Contains(c.ownership.owner);
                    }

                    if(relation == EntityRelation.inZone){
                        var owner = c.Group.GetComponent<GroupZone>().owner;
                        return values.Contains(owner);
                    }
                    return false;
                });
            }else{
                Debug.LogError($"input is invalid: {inputs?.GetType()?.Name??"null"}");
            }
            

            return false;
        }
    }

     [Serializable]
    public class ZoneCondition:BaseCondition
    {

       


        public GroupName zone;
        /// <summary>
        /// Comprueba si se cumple la condición
        /// </summary>
        public override bool check(object inputs, Context context){

            if(inputs is IEnumerable<object> collection){

                return collection.OfType<Card>()
                .All( c => c.Group.GetComponent<GroupZone>()?.zone == zone);
            }else{
                Debug.LogError($"input is invalid: {inputs?.GetType()?.Name??"null"}");
            }
            

            return false;
        }
    }
}