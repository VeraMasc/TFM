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
    /// Calcula un target relativo a otro
    /// </summary>
    [Serializable]
    public class EntityCardTargeter:EffectTargeter
    {
        [SerializeReference, SubclassSelector]
        public EffectTargeter source;

        public EntityRelation relation;
        

        public List<GroupName> zones=new();
        

        public override void resolveTarget(Effect.Context context){
            var origin = source.getTargets(context) as Entity[];
            //Get groups
            var zonedGroups = GameObject.FindObjectsOfType<GroupZone>()
                .Where(group => zones.Contains(group.zone) );
            
            var cards = zonedGroups.SelectMany(group => group.GetComponent<CardGroup>()?.MountedCards);

            switch(relation){
                case EntityRelation.controls: 
                    cards = cards.Where(card => origin.Contains(card?.GetComponent<CardOwnership>().controller));
                    break;
                case EntityRelation.owns: 
                    cards = cards.Where(card => origin.Contains(card?.GetComponent<CardOwnership>().owner));
                    break;
                default:
                    Debug.LogError("Invalid relation"); break;
                case EntityRelation.inZone: break;
            }
            _targets = cards.ToArray();
        }
        

        
    }

    public enum EntityRelation{
        inZone,
        controls,
        owns,
        
    }
}
