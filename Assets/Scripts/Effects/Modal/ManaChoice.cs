using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using Effect;
using Effect.Status;
using UnityEngine;


namespace Effect
{


    [Serializable]
    /// <summary>
    /// Permite seleccionar entre varias opciones de maná
    /// </summary>
    public class ManaChoice : EffectScript, IValueEffect
    {
        public int maxChoices=1;
        
        public List<ListContainer<Mana>> options = new();
            
        /// <summary>
        /// Opciones escogidas
        /// </summary>
        [NonSerialized]
        public List<int> chosen=new();


        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            List<int> ret = new();

                //Generar diálogo modal
                yield return UCoroutine.Yield(GameUI.singleton.getInput(GameUI.singleton.prefabs.cardSelectInput, 
                obj => {
                    ret = (List<int>)obj;
                },
                new InputParameters{ values= options.Select(o => o.ToArray()).ToArray(), 
                    extraConfig = new CardSelectInput.ExtraInputOptions(){
                        maxChoices = maxChoices,
                    }
                }));

                
                //Get chosen
                var results = ret.SelectMany( n => options[n].ToArray()).ToArray();
                

                //Guardar opciones SI escogidas
                context.previousValues.Add(results);
                context.choiceTreeIncrease();
        }
    }

}