using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


/// <summary>
/// Reusa el último set de targets utilizado
/// </summary>
[Serializable]
public class ReuseTargeter:EffectTargeter
{
    public override void resolveTarget(TargettingContext context){
        _targets = context.previousTargets?.LastOrDefault();
    }

}