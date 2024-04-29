using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;


/// <summary>
/// Se encarga de la generación de las cartas de acción
/// </summary>
public class ActionCard : MyCardSetup, IActionable
{

    
    

    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        base.Apply(data);
        if (data is ActionCardDefinition action)
        {
            effects = action.effects;
            effects = action.effects.cloneAll();
            effects.setContext(GetComponent<Card>());
            effects.refreshTriggerSuscriptions();
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

    
}

