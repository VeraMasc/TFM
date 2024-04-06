using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;


/// <summary>
/// Se encarga de la generación de las cartas de habitación
/// </summary>
public class RoomCard : MyCardSetup
{
    

    /// <summary>
    /// Texto que muestra el tamaño de la habitación
    /// </summary>
    public TextMeshPro sizeUI;

    /// <summary>
    /// Texto que muestra el número de salidas de la habitación
    /// </summary>
    public TextMeshPro exitsUI;
    
    public int size;

    public int exits;

   

    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        base.Apply(data);
        if (data is RoomCardDefinition roomCard)
        {
            size = roomCard.size;
            exits = roomCard.exits;
        }
        refreshValues();
    }

    [Button]
    public void refreshValues(){
        cardTextBox.text = cardText;
        sizeUI.text = size.ToString();
        exitsUI.text = exits.ToString();
    }
}

