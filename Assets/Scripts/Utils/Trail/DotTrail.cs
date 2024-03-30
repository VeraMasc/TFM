using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Ra.Trail;
using System;

/// <summary>
/// Para la ejecución en paralelo
/// </summary>
public class DotTrail : TrailObject<DotTrail>
{

    /// <summary>
    /// Ejecuta una corrutina
    /// </summary>
    public DotTrail Execute(IEnumerator enumerator, MonoBehaviour obj=null){
        obj ??= monoTrail;
        obj.StartCoroutine(enumerator);
        Wait(()=>enumerator.Current==null);
        
        return this;
    }

    
    public DotTrail Await(Coroutine coroutine){
        var enumerator = wrapperRoutine(coroutine);
        monoTrail.StartCoroutine(enumerator);
        Wait(()=>enumerator.Current==null);
        return this;
    }

    /// <summary>
    /// Ejecuta una acción en todos los elementos de una colección
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public DotTrail ForEach<T>(IEnumerable<T> enumerable, Action<T> action, double delay=0){
        T[] entries=new T[0];
        int i=0;
        return After(()=>{
            entries = enumerable.ToArray();
        }).While(() => {
            if(i >= entries.Length) //Break loop
                return false;
            //Execute action
            action.Invoke(entries[i]);
            i++;
            return true;
        },delay).Wait();
        //return this;
    }

    /// <summary>
    /// Espera a una corrutina
    /// </summary>
    private IEnumerator wrapperRoutine(Coroutine coroutine){
        yield return coroutine;
    }
}