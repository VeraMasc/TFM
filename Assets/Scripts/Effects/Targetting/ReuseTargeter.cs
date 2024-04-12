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
    /// <summary>
    /// Posición del elemento desde el último índice
    /// </summary>
    public int index;


    public override void storeTargets(Effect.Context context){
        //No guardar targets
    }

    public override void resolveTarget(Effect.Context context){
        
        _targets = extractTarget(context, index);
    }


    /// <summary>
    /// Extrae un valor de la pila de targets guardados
    /// </summary>
    /// <param name="index">Posición del elemento desde el último índice</param>
    /// <param name="destructive">Elimina el valor tras extraerlo</param>
    /// <returns></returns>
    public static ITargetable[] extractTarget(Effect.Context context, int index){
        int size;
        if((size = context.previousTargets.Count) <= index){
            Debug.LogError("Value to extract is out of bounds", (UnityEngine.Object) context.self);
            return new ITargetable[0];
        }

        var pos = size-index-1;
        var targets = context.previousTargets[pos];
        
        return targets;
        
    }
}