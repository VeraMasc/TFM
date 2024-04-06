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

    
    

    public ContentCardEffects effects;
    

    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        if (data is PokerCardDefinition pokerCard)
        {
            Image.sprite = pokerCard.Art;
            if (pokerCard.BackArt != null)
            {
                BackImage.sprite = pokerCard.BackArt;
            }
        }

        if (data is ContentCardDefinition contentCard)
        {
            gameObject.name = contentCard.cardName;
            Image.sprite = contentCard.Art ?? Image.sprite;
            if (contentCard.BackArt != null)
            {
                BackImage.sprite = contentCard.BackArt;
            }
            cardText = contentCard.cardText;

            effects = contentCard.effects;

        }
        refreshValues();
    }

    [Button]
    public void refreshValues(){
        cardTextBox.text = cardText;
    }

    
}

