using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;
using Effect;

/// <summary>
/// Marca un grupo como un tipo de zona concreta
/// </summary>
public class GroupZone : MonoBehaviour
{
    public GroupName zone;

    /// <summary>
    /// Ejecuta los triggers de entrada de una zona en la carta especificada
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public IEnumerator callEnterTrigger(Card card){

        switch (zone){
            case GroupName.Board:
                GameController.singleton.triggerManager.onEnter.invokeOn(card);
                break;
            
            case GroupName.Discard:
                GameController.singleton.triggerManager.onEnterDiscard.invokeOn(card);
                break;
            
            case GroupName.Exile:
                GameController.singleton.triggerManager.onEnterExile.invokeOn(card);
                break;

            case GroupName.Lost:
                GameController.singleton.triggerManager.onEnterLost.invokeOn(card);
                break;

            case GroupName.Deck:
                break;

            default: 
                Debug.LogError($"No enter event behavior defined for {zone}",card);
                break;
        }
        yield break;
    }
}
