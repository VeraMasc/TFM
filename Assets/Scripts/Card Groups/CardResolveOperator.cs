using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;
using CustomInspector;
using System.Linq;
using Common.Coroutines;

/// <summary>
/// Se encarga de ejecutar en orden los efectos/cartas en el stack
/// </summary>
public class CardResolveOperator : Activatable
{
    /// <summary>
    /// Grupo de cartas que resolver
    /// </summary>
    [SelfFill, ForceFill]
    public CardGroup stack;

    /// <summary>
    /// Indica si está resolviendo (esperando inputs) o esperando nuevas cartas/activación
    /// </summary>
    public bool resolve;

    /// <summary>
    /// Carta en proceso de resolución
    /// </summary>
    public Card currentCard;

    /// <summary>
    /// Indica el tipo de pila a la que se envia la carta al resolverse.
    /// Se resetea con cada carta.
    /// </summary>
    [ReadOnly]
    public GroupName sendTo = GroupName.Discard;

    /// <summary>
    /// Contexto actual de la resolución
    /// </summary>
    public TargettingContext context;
    protected override void OnActivate(){
        resolve = true;
    }

    /// <summary>
    /// Espera hasta que el stack esté libre
    /// </summary>
    public IEnumerator waitTillEmpty{
        get => UCoroutine.YieldAwait(()=> !resolve && stack.MountedCards.Count == 0);
    }

    void Update()
    {
        if(resolve && currentCard == null){
            StartCoroutine(resolveCard());
        }
    }

    /// <summary>
    /// Crea el contexto
    /// </summary>
    public void setContext(){
        context = new TargettingContext(currentCard);
    }

    /// <summary>
    /// Resuelve la siguiente carta de la secuencia
    /// </summary>
    /// <returns></returns>
    protected IEnumerator resolveCard(){
        if(!currentCard && stack.MountedCards.Count==0){
            resolve=false; //No hay nada más que resolver
            yield break;
        }
        //Get card
        currentCard ??= stack.MountedCards.Last();

        //Set up current card
        sendTo = GroupName.Discard;
        setContext();

        if(currentCard?.data is ContentCard content){
            yield return resolveContentCard(currentCard, content);
        }
        //TODO: Set targets

        //TODO: Execute effects for actions

        

        //Reset state
        currentCard=null;
        resolve =false;
    }

    /// <summary>
    /// Se encarga de la resolución de las cartas de contenido
    /// </summary>
    /// <param name="card">Carta en cuestión</param>
    /// <param name="content">Su contenido</param>
    protected IEnumerator resolveContentCard(Card card, ContentCard content){
        foreach(var effect in content.effects.revealEffect.list){
            effect.execute(this,context);
        }
        yield return sendToResolutionPile(card);
    }
    
    /// <summary>
    /// Obtiene la pila de resolución correspondiente y envía la carta allí.
    /// Pila definida por <see cref="sendTo"/>.
    /// </summary>
    /// <param name="card">Carta a enviar</param>
    /// <param name="playerIndex">índice de jugador a utilizar</param>
    /// <returns></returns>
    protected IEnumerator sendToResolutionPile(Card card, int? playerIndex=null){
        var group = GroupRegistry.Instance.Get(sendTo,playerIndex);
        yield return CardTransferOperator.sendCard(card,group);
    }
}
