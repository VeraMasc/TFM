using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Effect;
using CardHouse;

namespace Effect
{
    /// <summary>
    /// Devuelve todas las cartas de un tipo de zona concreta
    /// </summary>
    [Serializable]
    public class ZoneCardTargeter:EffectTargeter
    {
        

        public List<GroupName> zones=new();

        [SerializeReference, SubclassSelector]
        public BaseCondition condition;
        

        public override void resolveTarget(Effect.Context context){
            

            //Get groups
            var zonedGroups = GameObject.FindObjectsOfType<GroupZone>()
                .Where(group => zones.Contains(group.zone) );
            
            var cards = zonedGroups.SelectMany(group => group.GetComponent<CardGroup>()?.MountedCards);

            _targets = cards.ToArray();
        }
        

        
    }

    
}
