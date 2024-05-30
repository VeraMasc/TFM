using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using CustomInspector;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade cartas a una zona 
    /// </summary>
    [Serializable]
    public class CreateCards:Targeted
    {
        /// <summary>
        /// Cartas a duplicar //TODO: Card definitions as values
        /// </summary>
        [SerializeReference]
        public List<CardDefinition> cards;

        public Mode mode;

        

        // public override EffectScript clone(){
        //     var ret = new CreateCards(){
        //         cards = cards.ToList(),
        //         mode = mode
        //     };

            
        //     return ret;
        // }

        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Effect.Context context)
        {
            if(! (target is Entity entity))
                yield break;

            var creationManager = GameController.singleton.creationManager;
            var created = new List<Card>();
            foreach(var card in cards){
                var newCard = creationManager.create(card); //Create card
;               
                newCard.Scaling.StartSeeking(4);
                newCard.Homing.StartSeeking(Vector3.back *8);
                newCard.ownership.owner = entity;
                switch (mode){
                    case Mode.inPlace:
                        if(context.self is Card source){
                            var index = source.Group.MountedCards.FindIndex(c=> c == source);
                            if (index >=0){
                                source.Group.Mount(newCard,index+1);
                                
                            }
                        }
                        break;
                    case Mode.nowhere:

                        break;

                    //TODO: Implement other effect modes
                }
                
                created.Add(newCard);
                yield return new WaitForSeconds(0.4f);
            }
            context.previousTargets.Add(created.ToArray()); 
            
        }

        
        public enum Mode{
            /// <summary>
            /// Crea las cartas en el mismo sitio que la fuente del efecto
            /// </summary>
            inPlace,

            /// <summary>
            /// Crea las cartas en la mano del controlador
            /// </summary>
            inHand,

            /// <summary>
            /// Crea las cartas en el campo
            /// </summary>
            inBoard,

            /// <summary>
            /// No asigna las cartas a ningún grupo
            /// </summary>
            nowhere  
        }
    }
   
}
