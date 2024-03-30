using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;


/// <summary>
/// Se encarga de la generación de las cartas de habitación
/// </summary>
public class ContentCard : CardSetup, ITypedCard
{
    public SpriteRenderer Image;
    public SpriteRenderer BackImage;

    public TextMeshPro cardTextUI;
    
    public string cardText;
    public string cardName {get;set;}
    public HashSet<string> cardType {get;set;}

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
            Image.sprite = contentCard.Art ?? Image.sprite;
            if (contentCard.BackArt != null)
            {
                BackImage.sprite = contentCard.BackArt;
            }
            cardText = contentCard.cardText;

        }
        refreshValues();
    }

    [Button]
    public void refreshValues(){
        cardTextUI.text = cardText;
    }
}

