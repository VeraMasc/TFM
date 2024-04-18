using System;
using System.Collections;
using CustomInspector;
using UnityEngine;


namespace Effect.Value{
    /// <summary>
    /// Base de todos los parámetros de texto
    /// </summary>
    [Serializable]
    public class TextString:Value<string>
    {
        public TextString(){

        }
        public TextString(string init){
            value = init;
        }
    }
    
    /// <summary>
    /// Realiza un cálculo para obtener el valor
    /// </summary>
    [Serializable]
    public class TextStringCheck:TextString,  IDynamicValue<string>
    {
        public override bool isDynamic => false;
    }

    /// <summary>
    /// Pide al jugador un valor
    /// </summary>
    [Serializable]
    public class TextStringChoice : TextString, IDynamicChoiceValue<string>
    {
        public override bool isDynamic => false;

        [AsRange(-20,30)]
        public Vector2 range;

        [SerializeField]
        private bool _chooseOnCast;


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

