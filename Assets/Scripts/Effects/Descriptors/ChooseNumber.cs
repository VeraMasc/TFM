using UnityEngine;
using System;
using CustomInspector;

namespace Descriptor{
    /// <summary>
    /// Escoge un número
    /// </summary>
    [Serializable]
    public class ChooseNumber : ChoiceDescriptor
    {
        public int min;

        public int max;

       
        public int value;
    }
}