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
    /// Hace que el jugador robe cartas
    /// </summary>
    /// <param name="amount"> cuantas cartas ha de robar</param>
    public IEnumerator draw(int amount, Action<Card[]> returnAction = null){
        var cards =deck.Get(GroupTargetType.Last, amount);
        yield return CardTransferOperator.sendCards(cards,hand,0.5f);
        coReturn(returnAction, cards.ToArray());
    }

    /// <summary>
    /// El jugador descarta varias cartas. 
    /// </summary>
    /// <param name="amount">cuantas cartas ha de descartar</param>
    /// <returns></returns>
    public IEnumerator discard(int amount, Action<Card[]> returnAction = null){
        Card[] chosen= new Card[0];
        coReturn(returnAction, chosen);
        throw new NotImplementedException();

    }
    

    /// <summary>
    /// El jugador descarta cartas al azar
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="returnAction">Devuelve la lista de cartas descartadas</param>
    /// <returns></returns>
    public IEnumerator discardRandom(int amount, Action<Card[]> returnAction = null){
        var cards =hand.Get(GroupTargetType.Random, amount);
        yield return CardTransferOperator.sendCards(cards,discarded,0.5f);
        coReturn(returnAction, cards.ToArray());
    }

    /// <summary>
    /// El jugador pone cartas de su mazo en la pila de descarte
    /// </summary>
    /// <param name="amount">Cantidad de cartas a "moler"</param>
    /// <param name="returnAction">Devuelve la lista de cartas "molidas"</param>
    /// <returns></returns>
    public IEnumerator mill(int amount,  Action<Card[]> returnAction = null){
        var cards =deck.Get(GroupTargetType.Last, amount);
        yield return CardTransferOperator.sendCards(cards,discarded,0.5f);
        coReturn(returnAction, cards.ToArray());
    }
    
    /// <summary>
    /// Ejecuta una return action para devolver un valor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void coReturn<T>(Action<T> returnAction, T value){
        if(returnAction != null){
            returnAction.Invoke(value);
        }
    }

    /// <summary>
    /// Aplica un status a la entidad
    /// </summary>
    /// <param name="status"></param>
    public IEnumerator applyStatus(BaseStatus status, Action<BaseStatus> returnAction = null){
        throw new NotImplementedException();
    }

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