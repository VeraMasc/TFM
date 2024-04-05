using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using UnityEngine;

namespace CardHouse
{
    public class CardTransferOperator : Activatable
    {
        public GroupTransition Transition;
        public GroupTargetType GrabFrom = GroupTargetType.Last;
        public GroupTargetType SendTo = GroupTargetType.Last;
        public int NumberToTransfer = 1;
        public float FlipSpeed = 1;

        /// <summary>
        /// Transfiere las cartas de una en una
        /// </summary>
        public bool oneByOne; 

        public SeekerScriptable<Vector3> PopPushHomingOverride;

        public List<TimedEvent> OnSourceDepletedEventChain;

        public bool TryAgainAfterSourceDepleted;

        /// <summary>
        /// Corutina que ejecuta actualmente
        /// </summary>
        public Coroutine currentAction;
        

        protected override void OnActivate()
        {
            var cardsToMove = NumberToTransfer > 0 ? Transition.Source.Get(GrabFrom, NumberToTransfer) : Transition.Source.MountedCards.ToList();

            if(oneByOne){
                Coroutine self = null;
                currentAction = StartCoroutine(transferCoroutine(this,currentAction));
            }else{
                TransferCards(cardsToMove);
            }
                

            if (NumberToTransfer > cardsToMove.Count)
            {
                StartCoroutine(ExecuteOnSourceDepletedEventChain());
            }
        }

        IEnumerator ExecuteOnSourceDepletedEventChain()
        {
            yield return TimedEvent.ExecuteChain(OnSourceDepletedEventChain);
            if (TryAgainAfterSourceDepleted)
            {
                var cardsToMove = Transition.Source.Get(GrabFrom, NumberToTransfer);
                TransferCards(cardsToMove);
            }
        }


        void TransferCards(List<Card> cards)
        {
            foreach (var card in cards)
            {
                Transition.Destination.Mount(card, SendTo == GroupTargetType.First ? -1 : SendTo == GroupTargetType.Last ? null : UnityEngine.Random.Range(0, Transition.Destination.MountedCards.Count + 1), seekerSets: new SeekerSetList { new SeekerSet { Card = card, Homing = PopPushHomingOverride?.GetStrategy(), FlipSpeed = FlipSpeed } });
            }
        }


        /// <summary>
        /// Transfiere las cartas y espera a que lleguen a su destino
        /// </summary>
        /// <returns></returns>
        public static IEnumerator transferCoroutine(CardTransferOperator op, Coroutine self){
            var amount = op.NumberToTransfer;
            var transition = (GroupTransition) op.Transition.Clone();
            var grabFrom = op.GrabFrom;
            var sendTo = op.SendTo;
            yield return op.currentAction;
            for(var i=0; i<amount; i++)
            {
                //Get card
                var card = amount > 0 ? transition.Source.Get(grabFrom, 1).FirstOrDefault() : null;

                if(card==null)
                    break;

                //transfer and wait
                var resolved =false;
                transition.Destination.Mount(card, sendTo == GroupTargetType.First ? -1 : sendTo == GroupTargetType.Last ? null : UnityEngine.Random.Range(0, transition.Destination.MountedCards.Count + 1), seekerSets: new SeekerSetList { new SeekerSet { Card = card, Homing = op.PopPushHomingOverride?.GetStrategy(), FlipSpeed = op.FlipSpeed } });
                card.Homing.OnNextFinish.AddListener(()=>resolved = true);

                while(!resolved){
                    yield return null;
                }
            }
            
            
        }

        /// <summary>
        /// Corrutina que manda una carta a un grupo concreto
        /// </summary>
        /// <param name="card">Carta a enviar</param>
        /// <param name="destination">Grupo de destino</param>
        /// <param name="sendTo">posición de destino</param>
        /// <param name="homingOverride">Sobreescribe el homing de la carta</param>
        /// <param name="flipSpeed">Velocidad a la que se gira la carta</param>
        public static IEnumerator sendCard(Card card, CardGroup destination, GroupTargetType sendTo = GroupTargetType.Last, SeekerScriptable<Vector3> homingOverride = null, float flipSpeed = 1f){
            
            destination.Mount(card, sendTo == GroupTargetType.First ? -1 : sendTo == GroupTargetType.Last ? null : UnityEngine.Random.Range(0, destination.MountedCards.Count + 1), seekerSets: new SeekerSetList { new SeekerSet { Card = card, Homing = homingOverride?.GetStrategy(), FlipSpeed = flipSpeed } });

            //Esperar que llegue a su destino
            yield return UCoroutine.YieldAwait(()=>!card.Homing.seeking);
        }

        /// <summary>
        /// Envía múltiples cartas de un sitio a otro
        /// </summary>
        /// <param name="source">Grupo de origen</param>
        /// <param name="amount">Cantidad de cartas a transferir</param>
        /// <param name="destination">Grupo de destino</param>
        /// <param name="delay">Tiempo de espera entre cartas. Si es negativo, se transfieren en bloque</param>
        /// <param name="grabFrom">posición de la que sacar las cartas</param>
        /// <param name="sendTo">posición de destino</param>
        /// <param name="homingOverride">Sobreescribe el homing de la carta</param>
        /// <param name="flipSpeed">Velocidad a la que se gira la carta</param>
        /// <returns></returns>
        public static IEnumerator sendCardsFrom(CardGroup source, int amount, CardGroup destination, float delay, GroupTargetType grabFrom = GroupTargetType.Last, GroupTargetType sendTo = GroupTargetType.Last, SeekerScriptable<Vector3> homingOverride = null, float flipSpeed = 1f){
            //ToDO: lock groups during transfer
            var cardsToMove = source.Get(grabFrom, amount);

            foreach(var card in cardsToMove){
                var corroutine = UCoroutine.Yield(sendCard(card,destination,sendTo,homingOverride,flipSpeed));

                if (delay>=0){//Wait for transfer + delay
                    yield return corroutine;
                    yield return new WaitForSeconds(delay);
                }
            }
        }
    }
}