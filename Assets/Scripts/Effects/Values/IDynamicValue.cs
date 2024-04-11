using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect.Value{
    public interface IDynamicValue<T>
    {
        public T getValue(Context context);
        public virtual bool isDynamic{get=> false;}
    }

    public interface IDynamicChoiceValue<T>:IDynamicValue<T>, IManual
    {
        /// <summary>
        /// Waits until the player has chosen a value
        /// </summary>
        /// <returns></returns>
        public IEnumerator getPlayerChoice();
    }
}