using System;
using System.Collections;
using System.Collections.Generic;
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
        public List<CardSetupData> cards;

        public Mode mode;

       

        

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context)
        {
           throw new NotImplementedException();
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
    /// <summary>
    /// Contiene los datos necesarios para crear una carta //TODO: Crear generador de cartas que use el prefab necesario en base a la definición
    /// </summary>
    [Serializable]
    public struct CardSetupData{
        public CardDefinition definition;
        public GameObject cardPrefab;
    }
}
