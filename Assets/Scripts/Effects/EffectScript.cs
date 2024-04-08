using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using CardHouse;
using UnityEngine;

[Serializable]
public abstract class EffectScript
{
    /// <summary>
    /// Executes the effect
    /// </summary>
    public virtual IEnumerator execute(CardResolveOperator stack, Context context){
        throw new NotImplementedException();
        
    }
 
    public virtual EffectScript clone(){
        var ret = baseClone();
        return ret;
    }

    protected virtual EffectScript baseClone(){
        using (MemoryStream stream = new MemoryStream())
        {
            if (this.GetType().IsSerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as EffectScript;
            }
            return null;
        }
    }


    
}

