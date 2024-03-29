using System;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class AddContent : EffectScript
    {
        /// <summary>
        /// Quién roba las cartas
        /// </summary>
        [SerializeReference, SubclassSelector]
        EffectTargeter target;
        public int amount = 2;
    }
}
