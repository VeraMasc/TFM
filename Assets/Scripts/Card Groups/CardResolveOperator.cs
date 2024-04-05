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
        if(resolve){
            resolveCard();
        }
    }

    /// <summary>
    /// Crea el contexto
    /// </summary>
    public void setContext(){
        context = new TargettingContext(currentCard);
    }

    protected void resolveCard(){
        if(!currentCard && stack.MountedCards.Count==0){
            resolve=false; //No hay nada más que resolver
            return;
        }
        //Get card
        currentCard ??= stack.MountedCards.Last();

        //TODO: Set targets

        //TODO: Execute effects

        // var effects = currentCard.GetComponent<CardEffects>();

        // if(effects){
        //     effects.usageEffect.finishCasting(currentCard);
        // }

        //Reset state
        currentCard=null;
        resolve =false;
    }
    
}
