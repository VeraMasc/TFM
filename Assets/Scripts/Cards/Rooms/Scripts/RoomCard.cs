using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;


/// <summary>
/// Se encarga de la generación de las cartas de habitación
/// </summary>
public class RoomCard : CardSetup, ITypedCard
{
    public SpriteRenderer Image;
    public SpriteRenderer BackImage;

    /// <summary>
    /// Texto que muestra el tamaño de la habitación
    /// </summary>
    public TextMeshPro sizeUI;

    /// <summary>
    /// Texto que muestra el número de salidas de la habitación
    /// </summary>
    public TextMeshPro exitsUI;

    public TextMeshPro cardTextUI;
    
    public string cardText;
    public int size;

    public int exits;

    public string cardName {get;set;}
    public HashSet<string> cardType {get;set;}

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

        if (data is RoomCardDefinition roomCard)
        {
            Image.sprite = roomCard.Art ?? Image.sprite;
            if (roomCard.BackArt != null)
            {
                BackImage.sprite = roomCard.BackArt;
            }
            cardText = roomCard.cardText;
            size = roomCard.size;
            exits = roomCard.exits;

        }
        refreshValues();
    }

    [Button]
    public void refreshValues(){
        cardTextUI.text = cardText;
        sizeUI.text = size.ToString();
        exitsUI.text = exits.ToString();
    }
}

