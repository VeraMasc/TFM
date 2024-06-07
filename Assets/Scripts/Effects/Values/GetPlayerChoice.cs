using System;
using System.Collections;
using Common.Coroutines;
using UnityEngine;

namespace Effect{

    /// <summary>
    /// Obtiene la decisión del jugador y la guarda como valor
    /// </summary>
    [Serializable]
    public abstract class GetPlayerChoice : EffectScript, IValueEffect
    {
        //TODO: eliminar? Reworkear como Interfaz?
    }

    /// <summary>
    /// Obtiene la decisión numérica del jugador y la guarda como valor
    /// </summary>
    [Serializable]
    public class GetNumericChoice : GetPlayerChoice
    {
        /// <summary>
        /// Valor por defecto que poner en el input
        /// </summary>
        public int defaultValue; 
        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context){
            int result = defaultValue;
            
            var prefab = GameUI.singleton?.prefabs?.numericInput;
            if(prefab == null){
                Debug.LogError("Missing player input prefab");
                yield break;//No hay prefab para generar la interfaz
            }
            var parameters = new InputParameters(){
                text ="Choose a number",
            };
            yield return UCoroutine.Yield(GameUI.singleton.getInput(prefab, (value)=>{
                result = (int)value;
            },parameters));

            context.choiceTreeIncrease();
            context.previousValues.Add(result);
            yield break;
        }
    }

}