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
    public override void storeTargets(Context context){
        //No guardar targets
    }

    public override void resolveTarget(Context context){
        if(context.previousTargets.Count==0){
            Debug.LogError("No hay targets previos que poder reutilizar", (UnityEngine.Object)context.self);
        }
        _targets = context.previousTargets?.LastOrDefault();
    }

}