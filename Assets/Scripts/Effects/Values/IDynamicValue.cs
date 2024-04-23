using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effect.Value{
    public interface IDynamicValue<T>
    {
        public T getValue(Context context);
        public virtual bool isDynamic{get=> false;}
    }

    
}