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
    /// Indica si empezar a resolver
    /// </summary>
    public bool startResolve;

    /// <summary>
    /// Indica si está resolviendo 
    /// </summary>
    public bool resolving;

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
        startResolve = true;
    }


    /// <summary>
    /// Indica si el stack no está siendo usado ahora mismo
    /// </summary>
    public bool isEmpty => !resolving && stack.MountedCards.Count == 0;
    /// <summary>
    /// Espera hasta que el stack esté libre
    /// </summary>
    public IEnumerator waitTillEmpty{
        get => UCoroutine.YieldAwait(()=> !resolving && stack.MountedCards.Count == 0);
    }

    /// <summary>
    /// Espera hasta que el stack esté libre
    /// </summary>
    public IEnumerator waitTillOpen{
        get => UCoroutine.YieldAwait(()=> !(startResolve || resolving || precalculating));
    }

    public IEnumerator waitTillPrecalculated{
        get => UCoroutine.YieldAwait(()=> !precalculating);
    }

    void Update()
    {
        if(startResolve && activeCard == null){
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
    public IEnumerator precalculateCard(Card card){
        //Continuar solo si no se está precalculando
        yield return UCoroutine.YieldAwait(()=>!precalculating);
        precalculating = true;

        Coroutine triggers = null; //Triggers generados que hay que esperar al final del proceso

        if(card.data is MyCardSetup simpleCard 
            && (simpleCard.effects?.context?.precalculated != true))
        {   
            simpleCard.effects?.setContext(card);//Create context
            var effect = simpleCard.effects?.baseEffect;
            //Precalculate
            if(effect != null){
                yield return StartCoroutine(simpleCard.effects?.baseEffect.precalculate(simpleCard.effects.context, this));
            }
            
            //Return if cancelled
            if(simpleCard.effects?.context?.mode==ExecutionMode.cancel){
                if(simpleCard.effects.sourceGroup)
                    simpleCard.effects.sourceGroup.Mount(card);
                else{
                    stack.UnMount(card);
                    Destroy(simpleCard.gameObject);
                    yield return new WaitForSeconds(0.2f);
                }
                    
                goto end;
            }
            simpleCard.effects.context.precalculated = true;
            
            //Check if can be cast
            if(simpleCard is ActionCard action){
                var controller = action.effects.context.controller;
                //Cancel if can't be paid
                if(!controller.mana.tryToPay(action.effects.paidCost))
                    simpleCard.effects.context.mode= ExecutionMode.cancel;
            }

            //Return if cancelled
            if(simpleCard.effects?.context?.mode==ExecutionMode.cancel){
                simpleCard.effects.sourceGroup.Mount(card);
                goto end;
            }
            else
            { 
                var manager =TriggerManager.instance;
                //Trigger events
                if(simpleCard is ActionCard){
                    var data = new Card[]{card};
                    triggers = card.StartCoroutine(
                        manager.onUseAction.invokeWith(data)
                        .Then(manager.onCast.invokeOn(card))
                    );
                }
                //Reset priority
                resetPriority(card, simpleCard);
            }
        }
        
        Debug.Log("Precalculated",card);
    end:
        precalculating = false;
        yield return triggers;
    }

    private void resetPriority(Card card, MyCardSetup simpleCard)
    {
        if (card.data is TriggerCard trigger && !trigger.isActiveTrigger)
        { //Use turn priority for triggers
            GameMode.current.getPriorityOrder();
        }
        else
        {//Use response priority for actions
            var activeSide = simpleCard.effects.context.controller.team;
            GameMode.current.getResponsePriority(activeSide);
        }
    }

    /// <summary>
    /// Manda una carta al stack y la precalcula 
    /// </summary>
    /// <param name="card">Carta a enviar</param>
    public IEnumerator castCard(Card card, ManaCost alternativeCost = null, Entity caster=null){
        Debug.Log("Casting card");

        //Set cast cost
        if(card?.data is ActionCard action){ 
            action.effects.paidCost = alternativeCost?? action.cost;
        }
        if(caster && card.ownership){
            card.ownership.controller = caster;
        }
        //Cast
        stack.Mount(card);
        yield return UCoroutine.YieldAwait( ()=>!card.Homing.seeking);
        yield return new WaitForSeconds(0.1f);
        yield return waitTillPrecalculated;
        
    }

    /// <summary>
    /// Abre los modales de uso de la carta si los hay o la usa directamente
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public IEnumerator playerUseCard(Card card){
        Debug.Log("Using card");
        if(card.data is MyCardSetup setup){
            var modal = setup.tryCastAsModal();
            if(modal){
                yield return new WaitForEndOfFrame();
                yield return UCoroutine.YieldAwait(()=> !GameUI.singleton.activeUserInput);
                yield break;
            }
            else if( setup is  ActionCard action){
                action.effects.paidCost =action.cost;
                //Mirar si se puede lanzar de forma normal
                if(action.checkIfCastable(action.effects.context.controller)){
                    StartCoroutine(castCard(card));
                }
                    
                yield break;
            }
        }
        Debug.LogError($"Can't use non action card {card}",card);
    }


    /// <summary>
    /// Resuelve la siguiente carta de la secuencia
    /// </summary>
    /// <returns></returns>
    protected IEnumerator resolveCard(){
        startResolve=false; 
        if(!activeCard && stack.MountedCards.Count==0){
            //No hay nada más que resolver
            yield break;
        }
        resolving=true;
        //Get card
        activeCard = stack.MountedCards.Last();

        //Set up current card
        sendTo = GroupName.Discard;

        if(activeCard?.data is MyCardSetup simpleCard){
            context = simpleCard.effects.context;

            
            //Requerir que sea precalculada
            if(context?.precalculated != true){
                Debug.LogError($"Card '{context.self}' was not precalculated. Remember not to mount cards directly, use {nameof(castCard)}", (UnityEngine.Object)context.self);
                startResolve =false;
                yield break;
            }
            yield return StartCoroutine(resolveBaseEffect(activeCard, simpleCard));
        }
        

        //Reset state
        resolving =false;
        activeCard=null;
        GameUI.singleton?.viewFocusedTargeting(null);
        GameMode.current.checkState();
    }

    /// <summary>
    /// Se encarga de la resolución básica de las cartas
    /// </summary>
    /// <param name="card">Carta en cuestión</param>
    /// <param name="content">Su contenido</param>
    protected IEnumerator resolveBaseEffect(Card card,MyCardSetup content){
        foreach(var effect in content.getEffectsAs<BaseCardEffects>().baseEffect.list){
            yield return StartCoroutine(effect.execute(this,context));
            if(context.mode == ExecutionMode.cancel)
                break;
        }
        if(context.mode != ExecutionMode.cancel){
            yield return StartCoroutine(TriggerManager.instance?.onSpent.invokeOn(card));
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
        // if (zone){
        //     yield return StartCoroutine(zone.callEnterTrigger(card));
        // }
    }


    /// <summary>
    /// Crea una pseudo carta de trigger. Si se desea crearla en el stack usar <see cref="triggerEffect(Card, EffectChain)"/>
    /// </summary>
    /// <param name="source">Carta que ha causado el trigger</param>
    /// <param name="triggered">Cadena de efectos a desencadenar</param>
    /// <param name="at">Posición en la que generar la carta</param>
    /// <returns>Carta de trigger generada</returns>
    public Card createTriggerCard(Card source, EffectChain triggered, Transform at, bool isActive){
        var prefab = GameController.singleton.creationManager.triggerPrefab;
        var trigger = Instantiate(prefab, at.position - at.forward, at.rotation);
        var card = trigger.GetComponent<Card>();
        card.data = trigger; //Save trigger reference
        trigger.ApplyTrigger(source, triggered);
        trigger.isActiveTrigger = isActive; //Marcar como trigger activo o no
        return card;
    }

    /// <summary>
    /// Genera una carta de trigger y la pone en el stack
    /// </summary>
    /// <param name="source">Carta que ha causado el trigger</param>
    /// <param name="triggered">Cadena de efectos a desencadenar</param>
    /// <param name="isActive">Indica si es un trigger generado activamente o no</param>
    public IEnumerator triggerEffect(Card source, EffectChain triggered, bool isActive){
        yield return waitTillPrecalculated;
        var card = createTriggerCard(source,triggered, transform, isActive);
        Debug.Log(card.name,card);
        yield return StartCoroutine(castCard(card));
    }

    public IEnumerator triggerAbilityEffect(Card source, ITriggerDescriptor descriptor, bool isActive, Entity user= null)
    {
        yield return waitTillPrecalculated;
        var chain = EffectChain.cloneFrom(descriptor?.getEffects());
        var card = createTriggerCard(source,chain, transform, isActive);
        var trigger = (TriggerCard)card.data;

        //Change text
        var newText ="";

        if(descriptor.triggerId != string.Empty){
            trigger.cardTextBox?.ForceMeshUpdate(forceTextReparsing:true); //IMPORTANTE

            var chosenLink = trigger.cardTextBox.textInfo.linkInfo
                .Where( link => link.GetLinkID() == descriptor?.triggerId)
                .FirstOrDefault();

            if(chosenLink.textComponent != null){
                newText += chosenLink.getRawLinkText();
            }
        }
        if(descriptor is ImplicitTriggeredAbility implAb && implAb.text != string.Empty){
            var cardDef = (card.data as MyCardSetup).definition;
            var parsed = MyCardDefinition.parseCardText(implAb.text, cardDef);
            newText += parsed;
        }

        if(newText != String.Empty){
            trigger.changeText(newText);
        }
        

        //Execute
        Debug.Log(card.name,card);
        yield return StartCoroutine(castCard(card, caster:user));
    }
}
