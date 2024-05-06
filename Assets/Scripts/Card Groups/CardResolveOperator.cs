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
    /// Indica si está resolviendo 
    /// </summary>
    public bool resolve;

    /// <summary>
    /// Indica si está precalculando (esperando inputs)
    /// </summary>
    public bool precalculating;

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


     private static CardResolveOperator _singleton;
	///<summary>Controller Singleton</summary>
	public static CardResolveOperator singleton
	{
		get 
		{
			if (_singleton == null)
			{
				_singleton = FindObjectOfType<CardResolveOperator>(); //Para cuando el maldito hotreload me pierde la referencia
			}
			return _singleton;
		}
	}

    void Awake()
    {
        _singleton ??=this;
        if(_singleton != this)
            Destroy(this);
    }

    protected override void OnActivate(){
        resolve = true;
    }


    /// <summary>
    /// Indica si el stack no está siendo usado ahora mismo
    /// </summary>
    public bool isEmpty => !resolve && stack.MountedCards.Count == 0;
    /// <summary>
    /// Espera hasta que el stack esté libre
    /// </summary>
    public IEnumerator waitTillEmpty{
        get => UCoroutine.YieldAwait(()=> !resolve && stack.MountedCards.Count == 0);
    }

    /// <summary>
    /// Espera hasta que el stack esté libre
    /// </summary>
    public IEnumerator waitTillOpen{
        get => UCoroutine.YieldAwait(()=> !resolve && !precalculating &&
            (stack.MountedCards.Count == 0));
    }

    void Update()
    {
        if(resolve && activeCard == null){
            StartCoroutine(resolveCard());
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
    /// Precalcula la carta cuando entra en el stack
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    protected IEnumerator precalculateCard(Card card){
        precalculating = true;
        //TODO: Alternative Cast Modes
        //TODO: Set targets
        if(card.data is MyCardSetup simpleCard)
        {   
            simpleCard.effects?.setContext(card);//Create context
            var effect = simpleCard.effects?.baseEffect;
            if(effect != null){
                yield return StartCoroutine(simpleCard.effects?.baseEffect.precalculate(simpleCard.effects.context, this));
            }
            
        }
        precalculating = false;
    }

    /// <summary>
    /// Manda una carta al stack y la precalcula
    /// </summary>
    /// <param name="card">Carta a enviar</param>

    public IEnumerator castCard(Card card){
        Debug.Log("Casting card");
        stack.Mount(card);
        yield return UCoroutine.YieldAwait( ()=>!card.Homing.seeking);
        var routine = StartCoroutine(precalculateCard(card));
        yield return new WaitForSeconds(0.5f);
        yield return routine;
        
    }
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

        if(activeCard?.data is MyCardSetup simpleCard){
            context = simpleCard.effects.context;
            if(context == null){
                Debug.LogError($"Card was not precalculated. Remember not to mount cards directly, use {nameof(castCard)}",simpleCard);
                yield break;
            }
            Debug.Log(context.self);
            yield return StartCoroutine(resolveBaseEffect(activeCard, simpleCard));
        }
        

        //Reset state
        resolve =false;
        activeCard=null;
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
        yield return StartCoroutine(sendToResolutionPile(card));
    }
    
    /// <summary>
    /// Obtiene la pila de resolución correspondiente y envía la carta allí.
    /// Pila definida por <see cref="sendTo"/>.
    /// </summary>
    /// <param name="card">Carta a enviar</param>
    /// <param name="playerIndex">índice de jugador a utilizar</param>
    /// <returns></returns>
    protected IEnumerator sendToResolutionPile(Card card, int? playerIndex=null){
        if(card.data is TriggerCard trigger){ //Destroy triggers
            stack.UnMount(card);
            if(trigger!=null)
                Destroy(trigger.gameObject);
            yield return new WaitForSeconds(0.5f);
            yield break;
        }

        CardGroup group=null;
        if(card.data is MyCardSetup myCard ){ //Mira si la carta ya tiene destino asignado
            var context = myCard.effects?.context;
            group= context?.resolutionPile;
            //Asignar la pila de descarte en base al dueño de la carta
            
        }

        //Send to owner discard as default
        CardOwnership ownership;
        if(group == null && (ownership = card.GetComponent<CardOwnership>())){
            group = ownership?.owner?.discarded;
        }

        //Si todo falla, la manda a la pila por defecto
        group ??= GroupRegistry.Instance.Get(sendTo,null);
        if(group == null){
            Debug.LogError("Can't find default discard pile");
            stack.UnMount(card);
            Destroy(card.gameObject);
            yield break;
        }
        yield return StartCoroutine(CardTransferOperator.sendCard(card,group));
        var zone = group.GetComponent<GroupZone>();
        if (zone){
            yield return StartCoroutine(zone.callEnterTrigger(card));
        }
    }


    /// <summary>
    /// Crea una pseudo carta de trigger. Si se desea crearla en el stack usar <see cref="triggerEffect(Card, EffectChain)"/>
    /// </summary>
    /// <param name="source">Carta que ha causado el trigger</param>
    /// <param name="triggered">Cadena de efectos a desencadenar</param>
    /// <param name="at">Posición en la que generar la carta</param>
    /// <returns>Carta de trigger generada</returns>
    public Card createTriggerCard(Card source, EffectChain triggered, Transform at){
        var prefab = GameController.singleton.creationManager.triggerPrefab;
        var trigger = Instantiate(prefab, at.position, at.rotation);
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
    public IEnumerator triggerEffect(Card source, EffectChain triggered){
        var card = createTriggerCard(source,triggered, transform);
        Debug.Log(card.name,card);
        yield return StartCoroutine(castCard(card));
    }
}
