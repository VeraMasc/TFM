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
        [SerializeReference, SubclassSelector]
        public Numeric A;

        [SerializeReference, SubclassSelector]
        public Numeric B;

        public Operation operation;
        public enum Operation{
            suma,
            resta,
            multiplicación,
            división,
            min,
            max
        }

        public override IEnumerator execute(CardResolveOperator stack, Effect.Context context){
            int result;
            switch (operation){
                case Operation.suma:
                    result = A.getValue(context) + B.getValue(context);
                    break;

                case Operation.resta:
                    result = A.getValue(context) - B.getValue(context);
                    break;
                case Operation.multiplicación:
                    result = A.getValue(context) * B.getValue(context);
                    break;

                case Operation.división:
                    result = A.getValue(context) / B.getValue(context);
                    break;
                
                case Operation.min:
                    result = Mathf.Min(A.getValue(context),B.getValue(context));
                    break;

                case Operation.max:
                    result = Mathf.Max(A.getValue(context),B.getValue(context));
                    break;

                default:
                    Debug.LogError("Invalid operation value", (UnityEngine.Object)context.self);
                    result = 0;
                    break;

                
            }
            context.previousValues.Add(result);
            yield break;
        }
    }
    
}