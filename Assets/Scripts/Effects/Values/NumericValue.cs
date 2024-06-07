using System;
using System.Collections;
using CardHouse;
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
    public class NumericChoice : Numeric
    {
        public override bool isDynamic => true;

        [AsRange(-20,30)]
        public Vector2 range;



        public virtual IEnumerator awaitUserInput(Effect.Context context){
            var prefab = GameUI.singleton?.prefabs?.numericInput;
            if(prefab == null){
                Debug.LogError("Missing player input prefab");
                yield break;//No hay prefab para generar la interfaz
            }
            var parameters = new InputParameters(){
                text ="Choose a number",
            };
            yield return UCoroutine.Yield(GameUI.singleton.getInput(prefab, (value)=>{
                context.previousValues.Add(value);
            }, parameters));

            yield return null;
        }

     
    }
    /// <summary>
    /// Nivel del Skill actual
    /// </summary>
    [Serializable]
    public class SkillLevel : Numeric
    {
        public override int getValue(Context context){
            if(context.self is Card card && card.data is SkillCard skill){
                return this.value = skill.level;
            }else{
                Debug.LogError($"Can't get level of non skill card of type {context?.self?.GetType()}", (UnityEngine.Object)context?.self);
            }
            return 0;
        }
    }
}

