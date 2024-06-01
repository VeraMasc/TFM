using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;
using HorizontalLineAttribute = CustomInspector.HorizontalLineAttribute;
using System.Linq;
using Effect;
using System.Text.RegularExpressions;


/// <summary>
/// Se encarga de la generación de las cartas de acción
/// </summary>
public class ActionCard : MyCardSetup, IActionable
{

    [HorizontalLine(3, message ="Speed")]
    public SpeedTypes speedType;
    [NaughtyAttributes.Foldout("Components")]
    public TextMeshPro costText;
    public ManaCost cost;

    /// <summary>
    /// Objeto que muestra la velocidad de la carta
    /// </summary>
    [NaughtyAttributes.Foldout("Components")]
    public GameObject speedLabel;

    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        base.Apply(data);
        if (data is ActionCardDefinition action)
        {
            //Speed
            speedType = action.speedType;
            GameController.singleton.creationManager
                .replaceCardTypeline(GetComponent<Card>());
            
            //Effects
            effects = action.effects;
            effects = action.effects.cloneAll();
            effects.setContext(GetComponent<Card>());

            //Cost
            cost = action.cost;
            costText.text = MyCardDefinition.parseCost(cost.asCardText());
        }
        else {
            Debug.LogError($"Wrong Definition for ActionCard: {data?.GetType()?.Name}",data);
        }
        refreshValues();
    }
    
    [Button]
    public void refreshValues(){
        cardTextBox.text = cardText;
    }

    /// <summary>
    /// Comprueba si la acción tiene el timing adecuado para ser usada ahora mismo
    /// </summary>
    /// <returns></returns>
    public bool checkCastTiming(Entity user){
        if(speedType == SpeedTypes.Reaction || GameMode.current.disableTimingRestrictions)
            return true;

        return GameMode.current.isSpeedValid(user, speedType);
        
    }

    /// <summary>
    /// Comprueba si la acción cumple todos los requisitos para ser usada
    /// </summary>
    /// <returns></returns>
    public bool checkIfCastable(Entity user){

        return checkCastTiming(user) && user.mana.canPay(cost);
        
    }

    /// <summary>
    /// Comprueba si alguna habilidad de la carta tiene timing válido
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool checkActivationTiming(Entity user, GroupName? zone){
        return GameMode.current.disableTimingRestrictions || 
            effects.getAllAbilities()
                .OfType<ActivatedAbility>()
                .Where(ab => zone == null || ab.isActiveIn((GroupName)zone))
                .Any( ab => ab.checkActivationTiming(user));
    }
    

    
}

