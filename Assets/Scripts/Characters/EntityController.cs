using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;
using Effect;
using Effect.Status;
using System.Linq;

/// <summary>
/// Cualquier entidad que usa cartas de acción
/// </summary>
public partial class Entity : MonoBehaviour,ITargetable
{
    /// <summary>
    /// Salud del personaje
    /// </summary>
    public int health =20;

    /// <summary>
    /// Indica si la entidad sigue con vida
    /// </summary>
    public bool alive = true;
    
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

    /// <summary>
    /// Grupo de las cartas "en juego"
    /// </summary>
    public CardGroup board;

    [HorizontalLine(3f, message ="Data Management")]
    /// <summary>
    /// Definición del personaje
    /// </summary>
    public EntityData data;

    /// <summary>
    /// Estatus que actualmente afectan al personaje
    /// </summary>
    public List<Effect.Status.BaseStatus> statusEffects;

    [HorizontalLine(3f, message ="Interface")]

    /// <summary>
    /// EIndica el punto en el que se pone el marcador de target
    /// </summary>
    public Transform targeterTransform;


    /// <summary>
    /// Obtiene todos los targetables de la entidad (salvo ella misma). Principalmente sus cartas y efectos
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ITargetable> getMyTargetables(){
        //TODO: add cards on board
        //TODO: add abilities
        
        return Enumerable.Empty<ITargetable>();
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