using System;
using System.Collections;
using CustomInspector;
using UnityEngine;


namespace Effect.Value{
    /// <summary>
    /// Base de todos los parámetros numéricos
    /// </summary>
    [Serializable]
    public class Numeric:Value<int>
    {
        public Numeric(){

        }
        public Numeric(int init){
            value = init;
        }
    }
    
    /// <summary>
    /// Realiza un cálculo para obtener el valor
    /// </summary>
    [Serializable]
    public class NumericCheck:Numeric,  IDynamicValue<int>
    {
        public override bool isDynamic => false;
    }

    /// <summary>
    /// Pide al jugador un valor
    /// </summary>
    [Serializable]
    public class NumericChoice : Numeric, IDynamicChoiceValue<int>
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

