
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using CardHouse;
using Effect;
using UnityEngine;



[Serializable]
public class BaseCardEffects{
    /// <summary>
    /// Efectos que producir cuando la carta se "resuelve" (Esto significa cosas distintas dependiendo del tipo de carta)
    /// </summary>
    public EffectChain baseEffect;

    /// <summary>
    /// Lista de habilidades de la carta
    /// </summary>
    [SerializeReference, SubclassSelector]
    public List<Ability> abilities;

    /// <summary>
    /// Efectos estáticos 
    /// </summary>
    public EffectChain staticEffects;

    public Effect.Context context = null;

   

    [NaughtyAttributes.Button("Refresh Triggers")]
    /// <summary>
    /// Actualiza el estado de las suscripciones a los eventos de los triggers
    /// </summary>
    public void refreshTriggerSuscriptions(){
        if(context?.self== null){
            Debug.LogError("Can't subscribe triggered abilities without a context");
            return;
        }
            
        //TODO: add disabling of triggers based on zone
        foreach(var ability in abilities.OfType<TriggeredAbility>()){
            if(ability.source == null){
                //Initialize
                ability.source= (CardHouse.Card)(context?.self);
                ability.listener = (val) => ability.executeAbility(context,val);
                ability.trigger.subscribe(ability.source, ability.listener);
            }
            
        }
    }

    /// <summary>
    /// Crea el contexto. 
    /// </summary>
    public void setContext(Card card){

        if(card?.data is TriggerCard trigger){
            //Create context from source
            context = new Effect.Context(trigger.source);
        }else{
            context = new Effect.Context(card);
        }
        
    }
    

    public BaseCardEffects cloneAll(){
        var data = JsonUtility.ToJson(this);
        return (BaseCardEffects) JsonUtility.FromJson(data,this.GetType());
    }
}


