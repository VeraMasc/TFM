using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla quién tiene la propiedad de las distintas cartas
/// </summary>
[DisallowMultipleComponent]
public class CardOwnership : MonoBehaviour
{
    public Entity controller;

    public Entity owner;

    //TODO: Set ownership on deck setup

    public void setOwner(Entity owner){
        this.owner = controller = owner;
    }
}
