using UnityEngine;
using System;

namespace Descriptor{
    [Serializable]
    public class EffectValue<T>
    {
        public ValueName variable;
        public T Value;
    }

    /// <summary>
    /// Indica los distintos nombres de variables que se pueden usar en los efectos
    /// </summary>
    public enum ValueName{
        none,
        X,
        Y,
        Z,
        X2,
        Y2,
        Z3,
        choice,
        choice2,
        result,
        result2,

    }
}