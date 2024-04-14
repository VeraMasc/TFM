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
    public class CreateCards:EffectScript
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

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
            var creationManager = GameController.singleton.creationManager;
            foreach(var card in cards){
                var setup = creationManager.create(card);

                switch (mode){
                    case Mode.inPlace:
                        if(context.self is Card source){
                            var index = source.Group.MountedCards.FindIndex(c=> c == source);
                            if (index >=0){
                                var cardComp = setup.GetComponent<Card>();
                                source.Group.Mount(cardComp,index+1);
                            }
                        }
                        break;
                    //TODO: Implement other effect modes
                }
            }

            yield return new WaitForSeconds(0.5f);
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
        }
    }
   
}
