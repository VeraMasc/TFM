using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            
            // Debug.Log("transferencia Finalizada");
        }
    }
}