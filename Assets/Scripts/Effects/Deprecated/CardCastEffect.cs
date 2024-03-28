using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

/// <summary>
/// Efecto que se produce al usar una carta
/// </summary>
[Serializable]
public class CardCastEffect : BaseEffect
{
    /// <summary>
    /// Qué se hará con la carta al resolver el efecto
    /// </summary>
    public ResolutionMode goesTo;

    /// <summary>
    /// Acaba el proceso de cast y transfiere la carta a donde corresponda
    /// </summary>
    /// <param name="card"></param>
    public void finishCasting(Card card){
        
        //Destroy if needed
        if (goesTo == ResolutionMode.destroy){
            card.Group.UnMount(card);
            GameObject.Destroy(card);
        }

        //Get chosen pile name
        GroupName groupName = GroupName.None;
        switch(goesTo){
            case ResolutionMode.discard: groupName=GroupName.Discard; break;
        }

        //Put on chosen pile
        var group= GroupRegistry.Instance.Get(groupName, null);
        if (group == null) {
            Debug.LogWarning("Resolution cardgroup not found, discarding instead");
            group= GroupRegistry.Instance.Get(GroupName.Discard, null);
        }
        group.Mount(card,null, seekerSets: new SeekerSetList { new SeekerSet { Card = card} });
    }
}
