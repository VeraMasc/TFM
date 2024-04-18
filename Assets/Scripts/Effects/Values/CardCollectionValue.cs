using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using CardHouse;


namespace Effect.Value{
    /// <summary>
    /// Base de todos los parámetros de conjuntos de Cartas
    /// </summary>
    [Serializable]
    public class CardCollection:Value<IEnumerable<Card>>
    {
        public CardCollection(){

        }
        public CardCollection(IEnumerable<Card> init){
            value = init;
        }
    }
    
    /// <summary>
    /// Realiza un cálculo para obtener el valor
    /// </summary>
    [Serializable]
    public class CardCollectionCheck:CardCollection,  IDynamicValue<IEnumerable<Card>>
    {
        public override bool isDynamic => true;
    }


    /// <summary>
    /// Pide al jugador un valor
    /// </summary>
    [Serializable]
    public class CardCollectionChoice : CardCollection, IDynamicChoiceValue<IEnumerable<Card>>
    {
        public override bool isDynamic => false;

        [AsRange(-20,30)]
        public Vector2 range;




        public virtual IEnumerator awaitUserInput(Effect.Context context){
            //TODO: ask user for input
            yield return null;
        }

        public IEnumerator getPlayerChoice()
        {
            throw new NotImplementedException();
        }
    }
}

