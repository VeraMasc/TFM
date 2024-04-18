using System;
using System.Collections;
using Common.Coroutines;
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
        public override bool isDynamic => true;
    }

    /// <summary>
    /// Pide al jugador un valor
    /// </summary>
    [Serializable]
    public class NumericChoice : Numeric, IDynamicChoiceValue<int>
    {
        public override bool isDynamic => true;

        [AsRange(-20,30)]
        public Vector2 range;

    
        //TODO: Eliminar decisiones como valores


        public virtual IEnumerator awaitUserInput(Effect.Context context){
            var prefab = GameUI.singleton?.prefabs?.numericInput;
            if(prefab == null){
                Debug.LogError("Missing player input prefab");
                yield break;//No hay prefab para generar la interfaz
            }
            yield return UCoroutine.Yield(GameUI.singleton.getInput(prefab, (value)=>{
                Debug.Log(value);
            }));

            yield return null;
        }

        public IEnumerator getPlayerChoice()
        {
            throw new NotImplementedException();
        }
    }
}

