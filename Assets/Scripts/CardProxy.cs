using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Permite a una carta "estar en dos sitios a la vez" para poder interactuar con ella
/// </summary>
public class CardProxy : Card
{
    [HorizontalLine]
    [Button(nameof(moveRealCard))]
    /// <summary>
    /// Carta real a la que "sustituye"
    /// </summary>
    public Card realCard;

    /// <summary>
    /// Posición real de la carta "in world"
    /// </summary>
    public Vector3 realPos;


    /// <summary>
    /// Mueve la carta real y la coloca en el lugar del proxy
    /// </summary>
    public void moveRealCard(){
        if(realCard == null)
            return;

        if(realPos == null)
            realPos = realCard.Homing.getTarget(realCard.transform.position);
        realCard.displayHiding(false);
        realCard.Homing.StartSeeking(transform.position);
    }
}
