using System;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Effect;
using Effect.Duration;
using UnityEngine;
using UnityEngine.Events;



/// <summary>
/// Añade una habilidad a una carta en cuestión
/// </summary>
[Serializable]
public class AbilityModifier : BaseModifier
{
    [SerializeReference,SubclassSelector]
    public List<Ability> abilities;


    public virtual void onAbilityTrigger(Ability ability, Context context){
        // Debug.Log($"onAbilityTrigger {ability?.id}");
        if(ability?.id.StartsWith("removeSelf") == true){
            Debug.Log("Remove Modifier");
            removeSelf();
        }
    }

    
    
    
    public override void initialize(object initParams){

        refresh();
        
    }

    public override void refresh(){
        foreach(var ability in abilities){
            ability.modifier = this;
        }
    }
}

/// <summary>
/// Añade una habilidad temporalmente
/// </summary>
[Serializable]
public class TemporaryAbilityModifier : AbilityModifier
{

    public TemporaryAbilityModifier(){
        if(abilities?.Any() != true){
            addSelfDestruct();
        }
        
        
    }

    public TemporaryAbilityModifier addSelfDestruct(){
        var selfDestruct = new HiddenTriggeredAbility(){
            id="removeSelf",
            effects = new(),
            activeZoneList = new List<GroupName>(){GroupName.None},
            modifier = this,
        };

        abilities ??= new List<Ability>();
        abilities.Add(selfDestruct);
        return this;
    }
    
}

