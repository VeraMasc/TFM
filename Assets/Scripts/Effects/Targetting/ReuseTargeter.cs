using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Reusa el último set de targets utilizado
/// </summary>
[Serializable]
public class ReuseTargeter:EffectTargeter
{
    public override void resolveTarget(TargettingContext context){
        throw new  NotImplementedException();
    }

}