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

/*
    Poner en este archivo solo las funciones que han de ser llamadas frecuentemente por efectos y similares para interactuar con las entidades
*/
public partial class Entity 
{
    /// <summary>
    /// Cura a la entidad
    /// </summary>
    /// <param name="amount">Cantidad de vida a curar. Si es < 0 se ignora </param>
    /// <param name="returnAction">Devuelve la curación que realmente se ha efectuado</param>
    /// <returns></returns>
    public IEnumerator heal(int amount, bool overheal = true, Action<int> returnAction = null){
        //TODO: Replacement effects

        if(!overheal && amount > maxHealth-health){
                amount = maxHealth-health; //Prevent overheal
        }
        //Ignorar curación negativa
        if (amount>0){
            
            health += amount;
            
            yield return TriggerManager.instance.onHeal.invokeWith(this).Start(this);
            //TODO: add healing animation
            healthDisplay.onHealthChanged();
        }
        coReturn(returnAction, amount);
        yield break;
    }

    /// <summary>
    /// Intenta revivir a la entidad (falla si su vida es menor a 0)
    /// </summary>
    /// <param name="returnAction"></param>
    /// <returns></returns>
    public IEnumerator tryRevive(Action<bool> returnAction = null){
        
        if(health>0){
            forceRevive();

        }
        
        
        coReturn(returnAction, alive);
        yield break;
    }

    /// <summary>
    /// Fuerza al personaje a revivir, pueda mantenerse vivo o no
    /// </summary>
    /// <returns></returns>
    public IEnumerator forceRevive(){
        
        alive = true;
        healthDisplay.onHealthChanged();
        refreshTurnIndicator();
        yield return TriggerManager.instance.onRevive.invokeWith(this).Start(this);  

    }


    /// <summary>
    /// Daña a la entidad
    /// </summary>
    /// <param name="amount">Cantidad de vida a quitar. Si es < 0 se ignora </param>
    /// <param name="returnAction">Devuelve el daño que realmente se ha efectuado</param>
    /// <returns></returns>
    public IEnumerator damage(int amount, Action<int> returnAction = null){
        //TODO: Replacement effects
        if (amount>0){
            health -= amount;
            healthDisplay.onHealthChanged();

            yield return TriggerManager.instance.onDamaged.invokeWith(this).Start(this);
            //TODO: add damage animation
        }
        coReturn(returnAction, amount);
        yield break;
    }

    /// <summary>
    /// Mata a la entidad
    /// </summary>
    /// <param name="returnAction">Devuelve si se ha matado a la entidad</param>
    /// <returns></returns>
    public IEnumerator kill(Action<bool> returnAction = null){
        alive = false;
        var cards = hand.MountedCards.ToArray();
        yield return CardTransferOperator.sendCards(cards,lost,0.75f/cards.Count(),burstSend:true)
            .Start(this);
        
        if(this != CombatController.singleton?.currentTurn){ //Eliminar su turno de la lista de turnos
            CombatController.singleton.turnOrder.Remove(this);
        }
        
        //TODO: invoke death trigger
        yield break;
    }

    /// <summary>
    /// Hace que el jugador robe cartas
    /// </summary>
    /// <param name="amount"> cuantas cartas ha de robar</param>
    [NaughtyAttributes.Button()]
    public IEnumerator draw(int amount=1, float duration = 0.75f, Action<Card[]> returnAction = null){
        if(amount <=0)
            yield break;
        var cards =deck.Get(GroupTargetType.Last, amount);
        yield return StartCoroutine( 
            CardTransferOperator.sendCards(cards,hand, duration/amount, burstSend:true)
        );
        if(amount>cards.Count && deck.MountedCards.Count==0)
            yield return kill().Start(this);
        yield return TriggerManager.instance.onDraw.invokeWith(this);
        coReturn(returnAction, cards.ToArray());
        yield break;
    }

    /// <summary>
    /// Roba cartas pero no cuenta como robar
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="duration"></param>
    /// <param name="returnAction"></param>
    /// <returns></returns>
    public IEnumerator pseudoDraw(int amount=1, float duration = 0.75f, Action<Card[]> returnAction = null){
        if(amount <=0)
            yield break;
        var cards =deck.Get(GroupTargetType.Last, amount);
        yield return StartCoroutine( 
            CardTransferOperator.sendCards(cards,hand, duration/amount, burstSend:true)
        );
        
        coReturn(returnAction, cards.ToArray());
        yield break;
    }

   

    /// <summary>
    /// Mete cartas en el fondo del mazo y roba la misma cantidad
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="duration"></param>
    /// <param name="returnAction"></param>
    /// <returns></returns>
    public IEnumerator reDraw(Card[] cards ,float duration = 0.75f, Action<Card[]> returnAction = null){
        var chosen = hand.MountedCards.Intersect(cards).ToArray();
        if(chosen.Count() >0){
            yield return StartCoroutine(CardTransferOperator.sendCards(chosen,deck,duration/chosen.Count(),sendTo:GroupTargetType.First, burstSend:true));

            yield return StartCoroutine(pseudoDraw(chosen.Count(),duration));
        }

        coReturn(returnAction, chosen);


    }
    
    public IEnumerator shuffle(){
        foreach(var card in deck.MountedCards){
            card.SetFacing(CardFacing.FaceDown,true);
        }
        deck.Shuffle();
        yield return new WaitForSeconds(0.3f);
    }


     /// <summary>
    /// El jugador descarta varias cartas. 
    /// </summary>
    /// <param name="amount">cuantas cartas ha de descartar</param>
    /// <returns></returns>
    public IEnumerator discardCards(Card[] cards ,float duration = 0.75f, Action<Card[]> returnAction = null){
        var chosen = hand.MountedCards.Intersect(cards).ToArray();
        if(chosen.Count() >0){
            yield return StartCoroutine(CardTransferOperator.sendCards(chosen,discarded,duration/chosen.Count(),burstSend:true));
        }

        coReturn(returnAction, chosen);


    }

    /// <summary>
    /// El jugador descarta cartas al azar
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="returnAction">Devuelve la lista de cartas descartadas</param>
    /// <returns></returns>
    public IEnumerator discardRandom(int amount, Action<Card[]> returnAction = null){
        var cards =hand.Get(GroupTargetType.Random, amount);
        yield return StartCoroutine(CardTransferOperator.sendCards(cards,discarded,0.5f));
        coReturn(returnAction, cards.ToArray());
    }

    public IEnumerator enforceHandSize(Action<Card[]> returnAction = null){
        var diff = hand.MountedCards.Count - maxHandSize;
        if(diff>0){
            yield return StartCoroutine(Discard.discardChoice(this,diff));
        }
    }

    /// <summary>
    /// El jugador pone cartas de su mazo en la pila de descarte
    /// </summary>
    /// <param name="amount">Cantidad de cartas a "moler"</param>
    /// <param name="returnAction">Devuelve la lista de cartas "molidas"</param>
    /// <returns></returns>
    [NaughtyAttributes.Button()]
    public IEnumerator mill(int amount=1,  Action<Card[]> returnAction = null){
        var cards =deck.Get(GroupTargetType.Last, amount);
        yield return StartCoroutine(CardTransferOperator.sendCards(cards,discarded,0.75f/amount,flipSpeed:4f,burstSend:true));
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
    /// Recupera los permanentes que tiene esta entidad en propiedad
    /// </summary>
    public IEnumerable<ActionCard> myPermanents {
        get {
            return CombatController.singleton?.board?.MountedCards?.Select(card => card.data)
                .OfType<MyCardSetup>()
                .Where(data => data?.effects?.context?.controller == this)
                .Cast<ActionCard>();
        }
    }

    /// <summary>
    /// Selecciona esta entidad como jugador actual si es posible
    /// </summary>
    public void trySelectPlayer(){
        if(team != EntityTeam.player)
            return;
        
        GameMode.current.setSelectedPlayer(this);
    }
}