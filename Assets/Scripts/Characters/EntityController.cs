using System.Collections;
using System.Collections.Generic;
using CardHouse;

using UnityEngine;

/// <summary>
/// Cualquier entidad que usa cartas de acción
/// </summary>
public class Entity : MonoBehaviour,ITargetable
{
    /// <summary>
    /// Mano con cartas de la entidad
    /// </summary>
    public CardGroup hand;

    public CardGroup deck;

    public EntityTeam team;


    /// <summary>
    /// Hace que el jugador robe cartas
    /// </summary>
    /// <param name="amount"> cuantas cartas ha de robar</param>
    public void draw(int amount){
        //TODO: Card transfer
    }

    void OnEnable()
    {
        //Add entity to list of entities
        GameController.singleton.entities.Add(this);
    }
    
    void OnDisable()
    {
        //Remove entity from list
        GameController.singleton.entities.Remove(this);
    }
}


public enum EntityTeam{
    none,
    player,
    enemy
}