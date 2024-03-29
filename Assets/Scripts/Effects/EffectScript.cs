using System;
using System.Collections;
using CardHouse;
using UnityEngine;

[Serializable]
public abstract class EffectScript 
{
    /// <summary>
    /// Executes the effect
    /// </summary>
    public virtual IEnumerator execute(Card self){
        throw new NotImplementedException();
        
    }
}