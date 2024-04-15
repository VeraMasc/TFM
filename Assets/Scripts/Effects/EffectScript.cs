using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using CardHouse;
using Effect;
using UnityEngine;
using Unity.Properties;

[Serializable]
public abstract class EffectScript
{
    /// <summary>
    /// Executes the effect
    /// </summary>
    public virtual IEnumerator execute(CardResolveOperator stack, Effect.Context context){
        throw new NotImplementedException();
        
    }
 
    public virtual EffectScript clone(){
        var ret = baseClone();
        return ret;
    }

    /// <summary>
    /// Obtiene todas las partes del efecto que requieren inputs del jugador
    /// </summary>
    /// <returns> lista de los inputs recuperados</returns>
    public virtual List<IManual> getManualInputs(){
        return new List<IManual>();
    }

    protected virtual EffectScript baseClone(){
       
        var data = JsonUtility.ToJson(this);
        return (EffectScript) JsonUtility.FromJson(data,this.GetType());
    }


    
}

