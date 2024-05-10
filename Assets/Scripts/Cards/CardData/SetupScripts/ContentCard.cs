using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;


/// <summary>
/// Se encarga de la generación de las cartas de habitación
/// </summary>
public class ContentCard : MyCardSetup
{

    
    

    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        base.Apply(data);
        if (data is ContentCardDefinition contentCard)
        {
            effects = contentCard.effects.cloneAll();
            effects.setContext(GetComponent<Card>());
            effects.refreshAbilitySuscriptions(GroupName.None);
        }
        refreshValues();
    }
    
    [Button]
    public void refreshValues(){
        cardTextBox.text = cardText;
    }

    
}

