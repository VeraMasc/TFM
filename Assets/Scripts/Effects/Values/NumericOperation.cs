using UnityEngine;

using System;
using Effect.Value;
using System.Collections;

namespace Effect{
    /// <summary>
    /// Realiza operaciones numéricas
    /// </summary>
    [Serializable]
    public class NumericOperation:EffectScript, IValueEffect
    {
        public Numeric A;

        public Numeric B;

        public Operation operation;
        public enum Operation{
            suma,
            resta,
            multiplicación,
            división
        }

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context){
            int result;
            switch (operation){
                case Operation.suma:
                    result = A.getValue() + B.getValue();
                    break;

                case Operation.resta:
                    result = A.getValue() - B.getValue();
                    break;
                case Operation.multiplicación:
                    result = A.getValue() * B.getValue();
                    break;

                case Operation.división:
                    result = A.getValue() / B.getValue();
                    break;
                
                default:
                    Debug.LogError("Invalid operation value", (UnityEngine.Object)context.self);
                    result = 0;
                    break;

                
            }
            context.previousValues.Add(new object[]{result});
            yield break;
        }
    }
}