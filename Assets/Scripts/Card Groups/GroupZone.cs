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

    public Entity owner;

    /// <summary>
    /// Ejecuta los triggers de entrada de una zona en la carta especificada
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public IEnumerator callEnterTrigger(Card card){

        var trigger = GameController.singleton.triggerManager;
        switch (zone){
            case GroupName.Board:
                yield return StartCoroutine(trigger.onEnter.invokeOn(card));
                break;
            
            case GroupName.Discard:
                yield return StartCoroutine(trigger.onEnterDiscard.invokeOn(card));
                break;
            
            case GroupName.Exile:
                yield return StartCoroutine(trigger.onEnterExile.invokeOn(card));
                break;

            case GroupName.Lost:
                yield return StartCoroutine(trigger.onEnterLost.invokeOn(card));
                break;

            case GroupName.Deck:
                break;
            case GroupName.Hand:
                break;
            case GroupName.Stack:
                break;
            case GroupName.Skills:
                break;
            case GroupName.None:
                break;
            default: 
                Debug.LogError($"No enter event behavior defined for {zone}",card);
                break;
        }
        yield break;

        
    }

    public IEnumerator callLeaveTrigger(Card card){
        var trigger = GameController.singleton.triggerManager;
        yield return StartCoroutine(trigger.onLeave.invokeOn(card));
    }
}
