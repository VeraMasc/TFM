
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using CardHouse;
using Common.Coroutines;
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
    public List<Ability> abilities = new();

    /// <summary>
    /// Efectos estáticos 
    /// </summary>
    public EffectChain staticEffects;

    public Effect.Context context;

    /// <summary>
    /// Indica de donde viene la carta o el efecto
    /// </summary>
    public GroupName sourceZone;

    public CardGroup sourceGroup;


   

    [NaughtyAttributes.Button("Refresh Triggers")]
    /// <summary>
    /// Actualiza el estado de las suscripciones a los eventos de los triggers y permite activar las activated abilities
    /// </summary>
    public void refreshAbilitySuscriptions(GroupName zone){
        if(context?.self== null){
            Debug.LogError("Can't subscribe triggered abilities without a context");
            return;
        }
        //TODO: add disabling of triggers based on zone
        foreach(var ability in abilities){
            //Initialize if needed
            ability.source ??= (CardHouse.Card)(context?.self);
            if(ability is TriggeredAbility triggered)
            {
                //Initialize trigger if needed
                triggered.listener ??= (val) => UCoroutine.Yield(triggered.executeAbility(context,val));
            }
            
            ability.onChangeZone(zone);
            
            
        }
    }

    /// <summary>
    /// Crea el contexto. 
    /// </summary>
    public void setContext(Card card){

        Card self = card;
        var effector= card;

        if(card?.data is TriggerCard trigger){
            //Create context from source
            self = trigger.source;
        }

        CardOwnership ownership = self?.GetComponent<CardOwnership>();
        if(ownership){
            context = new Effect.Context(self, ownership, effector);
        }
        else{
            context = new Effect.Context(self,effector);
        }
        
    }
    

    public BaseCardEffects cloneAll(){
        var data = JsonUtility.ToJson(this);
        return (BaseCardEffects) JsonUtility.FromJson(data,this.GetType());
    }
}


