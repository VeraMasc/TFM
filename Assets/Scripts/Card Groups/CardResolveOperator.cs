using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;
using CustomInspector;
using System.Linq;
using Common.Coroutines;
using Effect;
using System;

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
    public Card activeCard;

    /// <summary>
    /// Primera carta en orden de resolución
    /// </summary>
    public Card topCard{get => stack?.MountedCards?.LastOrDefault();}

    /// <summary>
    /// Indica el tipo de pila a la que se envia la carta al resolverse.
    /// Se resetea con cada carta.
    /// </summary>
    [ReadOnly]
    public GroupName sendTo = GroupName.Discard;

    /// <summary>
    /// Contexto actual de la resolución
    /// </summary>
    public Effect.Context context;

    /// <summary>
    /// Inputs de la carta actual que faltan por llenar
    /// </summary>
    [NonSerialized]
    public List<IManual> userInputs;

    /// <summary>
    /// Currentl manual input waiting for the player
    /// </summary>
    [SerializeReference, SubclassSelector]
    public IManual currentUserInput;

    [SelfFill(true)]
    public StackUI stackUI;

    /// <summary>
    /// Prefab used to generate Triggers
    /// </summary>
    [AssetsOnly]
    public TriggerCard triggerPrefab;
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
        if(resolve && activeCard == null){
            StartCoroutine(resolveCard());
        }
    }

    /// <summary>
    /// Crea el contexto
    /// </summary>
    public void setContext(){
        if(activeCard?.data is TriggerCard trigger){
            //Create context from source
            context = new Effect.Context(trigger.source);
        }else{
            context = new Effect.Context(activeCard);
        }
        
    }

    public IEnumerator getUserInputs(){
        //TODO: await for inputs on cast
        //TODO: await for inputs on resolution
        throw new NotImplementedException();
    }

    /// <summary>
    /// Espera a que se acaben de poner todos los inputs necesarios
    /// </summary>
    public IEnumerator userInputsFinished => UCoroutine.YieldAwait(
        ()=>(userInputs?.Count ?? 0) == 0
    );

    /// <summary>
    /// Resuelve la siguiente carta de la secuencia
    /// </summary>
    /// <returns></returns>
    protected IEnumerator resolveCard(){
        if(!activeCard && stack.MountedCards.Count==0){
            resolve=false; //No hay nada más que resolver
            yield break;
        }
        //Get card
        activeCard ??= stack.MountedCards.Last();

        //Set up current card
        sendTo = GroupName.Discard;
        setContext();
        Debug.Log(context.self);

        //TODO: Alternative Cast Modes
        //TODO: Set targets

        if(activeCard?.data is MyCardSetup simpleCard){
            yield return resolveBaseEffect(activeCard, simpleCard);
        }
        

        //Reset state
        resolve =false;
    }

    /// <summary>
    /// Se encarga de la resolución básica de las cartas
    /// </summary>
    /// <param name="card">Carta en cuestión</param>
    /// <param name="content">Su contenido</param>
    protected IEnumerator resolveBaseEffect(Card card, MyCardSetup content){
        foreach(var effect in content.getEffectsAs<BaseCardEffects>().baseEffect.list){
            yield return StartCoroutine(effect.execute(this,context));
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
        activeCard=null;
        if(card.data is TriggerCard trigger){ //Destroy triggers
            stack.UnMount(card);
            Destroy(trigger.gameObject);
            yield return new WaitForSeconds(0.5f);
        }
        var group = GroupRegistry.Instance.Get(sendTo,playerIndex);
        yield return CardTransferOperator.sendCard(card,group);
    }


    /// <summary>
    /// Crea una pseudo carta de trigger. Si se desea crearla en el stack usar <see cref="triggerEffect(Card, EffectChain)"/>
    /// </summary>
    /// <param name="source">Carta que ha causado el trigger</param>
    /// <param name="triggered">Cadena de efectos a desencadenar</param>
    /// <param name="at">Posición en la que generar la carta</param>
    /// <returns>Carta de trigger generada</returns>
    public Card createTriggerCard(Card source, EffectChain triggered, Transform at){
        var trigger = Instantiate(triggerPrefab, at.position, at.rotation);
        var card = trigger.GetComponent<Card>();
        card.data = trigger; //Save trigger reference
        trigger.ApplyTrigger(source, triggered);
        return card;
    }

    /// <summary>
    /// Genera una carta de trigger y la pone en el stack
    /// </summary>
    /// <param name="source">Carta que ha causado el trigger</param>
    /// <param name="triggered">Cadena de efectos a desencadenar</param>
    public void triggerEffect(Card source, EffectChain triggered){
        var card = createTriggerCard(source,triggered, transform);
        stack.Mount(card);
    }
}
