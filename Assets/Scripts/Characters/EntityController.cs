using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Cualquier entidad que usa cartas de acción
/// </summary>
public class Entity : MonoBehaviour,ITargetable
{
    /// <summary>
    /// Salud del personaje
    /// </summary>
    public int health =20;
    
    /// <summary>
    /// Equipo del personaje
    /// </summary>
    public EntityTeam team;

    /// <summary>
    /// Cantidad de cartas que puede tener en la mano al terminar el turno
    /// </summary>
    public int maxHandSize =4;

    [HorizontalLine(3f, message ="Card Management")]
    /// <summary>
    /// Mano con cartas de la entidad
    /// </summary>
    [ForceFill]
    public CardGroup hand;

    /// <summary>
    /// Mazo del personaje
    /// </summary>
    [ForceFill]
    public CardGroup deck;

    /// <summary>
    /// Pila de descarte del personaje
    /// </summary>
    [ForceFill]
    public CardGroup discarded;

    /// <summary>
    /// Pila de exilio del personaje
    /// </summary>
    [ForceFill]
    public CardGroup exile;

    /// <summary>
    /// Pila de cartas enganchadas al personaje
    /// </summary>
    public CardGroup attached;

    /// <summary>
    /// Skills
    /// </summary>
    public CardGroup skills;

    [HorizontalLine(3f, message ="Data Management")]
    /// <summary>
    /// Definición del personaje
    /// </summary>
    public EntityData data;

    


    /// <summary>
    /// Hace que el jugador robe cartas
    /// </summary>
    /// <param name="amount"> cuantas cartas ha de robar</param>
    public IEnumerator draw(int amount, Action<Card[]> returnAction = null){
        //TODO: Card transfer
        
        yield return CardTransferOperator.sendCardsFrom(deck,amount,hand,0);
        // throw new NotImplementedException();
    }

    /// <summary>
    /// El jugador descarta varias cartas. 
    /// </summary>
    /// <param name="amount">cuantas cartas ha de descartar</param>
    /// <returns>Devuelve la lista de cartas descartadas</returns>
    public IEnumerator discard(int amount, Action<Card[]> returnAction = null){
        throw new NotImplementedException();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount">Devuelve la lista de cartas descartadas</param>
    /// <returns></returns>
    public Card[] discardRandom(int amount, Action<Card[]> returnAction = null){
        throw new NotImplementedException();
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