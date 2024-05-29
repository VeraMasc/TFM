
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

    /// <summary>
    /// Momento en el que se produjo el cambio de zona (en segundos desde inicio del juego)
    /// </summary>
    public double entryTime;

    /// <summary>
    /// Coste que se ha pagado para usar esta carta 
    /// </summary>
    public ManaCost paidCost =null;


   

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
        foreach(var ability in getAllAbilities()){
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


    public IEnumerable<Ability> getAllAbilities(){
        var mods = CardModifiers.getModifiers((CardHouse.Card)(context?.self))
            .OfType<AbilityModifier>();
        var modAbs =mods.SelectMany(m=>m.abilities);
        var ret = abilities.Concat(modAbs);
        return ret;
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

        CardOwnership ownership = effector?.ownership;
        if(ownership){
            context = new Effect.Context(self, ownership, effector);
        }
        else{
            context = new Effect.Context(self,effector);
        }
        context.mode = ExecutionMode.normal;
        
    }
    

    public BaseCardEffects cloneAll(){
        var data = JsonUtility.ToJson(this);
        return (BaseCardEffects) JsonUtility.FromJson(data,this.GetType());
    }
}


