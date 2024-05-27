using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Effect;
using CardHouse;
using Effect.Value;

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
        public EffectTargeter exclude;

        [SerializeReference, SubclassSelector]
        public BaseCondition condition;

        
        

        public override void resolveTarget(Effect.Context context){
            

            //Get groups
            var zonedGroups = GameObject.FindObjectsOfType<GroupZone>()
                .Where(group => zones.Contains(group.zone) );
            
            var cards = zonedGroups.SelectMany(group => group.GetComponent<CardGroup>()?.MountedCards);

            //Si hay que excluir ciertas cartas
            if(exclude != null){
                var excluded = exclude.getTargets(context);
                cards = cards.Except(excluded).Cast<Card>();
            }

            _targets = cards.ToArray();
        }
        

        
    }

    /// <summary>
    /// Devuelve N cartas del top del mazo
    /// </summary>
    [Serializable]
    public class DeckTop:EffectTargeter
    {
        [SerializeReference, SubclassSelector]
        public Numeric amount;
        public override void resolveTarget(Effect.Context context){
            int amountVal = (int)(amount.getValueObj(context));

            if(context.controller== null || amountVal<=0)
                return;
            
            var cards =context.controller.deck.Get(GroupTargetType.Last, amountVal);
            _targets = cards.ToArray();
        }
    }

    
}
