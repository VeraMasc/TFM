using System;
using UnityEngine;

[Serializable]
public abstract class EffectScript 
{
    /// <summary>
    /// Executes the effect
    /// </summary>
    public virtual void execute(){
        throw new NotImplementedException();
    }
}