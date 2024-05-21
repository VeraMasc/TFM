using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;
using HorizontalLineAttribute = CustomInspector.HorizontalLineAttribute;


/// <summary>
/// Se encarga de la generación de las cartas de acción
/// </summary>
public class ActionCard : MyCardSetup, IActionable
{

    [HorizontalLine(3, message ="Speed")]
    public SpeedTypes speedType;

    /// <summary>
    /// Objeto que muestra la velocidad de la carta
    /// </summary>
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
        if(speedType == SpeedTypes.Reaction)
            return true;

        return GameMode.current.isSpeedValid(user, speedType);
        
    }
    
}

