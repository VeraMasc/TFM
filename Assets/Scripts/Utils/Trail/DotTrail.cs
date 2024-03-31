using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Ra.Trail;
using System;
using Ra.Trail.Works;

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
        var routine = obj.StartCoroutine(enumerator);
        
        return Await(routine);
    }

    
    public DotTrail Await(Coroutine coroutine){
        bool endVal = false;
        var enumerator = wrapperRoutine(coroutine, ()=>endVal=true);
        monoTrail.StartCoroutine(enumerator);
        
        return Wait(()=>{
            Debug.Log(enumerator.Current);
            return endVal;
        });
    }

    /// <summary>
    /// Espera a una corrutina
    /// </summary>
    private IEnumerator wrapperRoutine(Coroutine coroutine, Action end){
        yield return coroutine;
        end.Invoke();
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
        //TODO: Rework to use nesting (.End)
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

    public DotTrail ForEach2<T>(IEnumerable<T> enumerable, string varName){
        Direct(new WorkOptions
        {
            action = data =>
            {
                monoTrail.StartCoroutine( ForEachEnumerator(enumerable, data, v=>{
                    this[varName].Get.value = v;
                } ));                
                
                return null;
            },
            openBracket = true
        });
        return this;
    }
    
    /// <summary>
    /// Based in ForEnumerator
    /// </summary>
    /// <param name="times"></param>
    /// <param name="delay"></param>
    /// <param name="element"></param>
    /// <returns></returns>
    private IEnumerator ForEachEnumerator<T>(IEnumerable<T> enumerable, SequenceElement element, Action<T> setValue)
        {
            foreach (var entry in enumerable)
            {
                setValue(entry);
                monoTrail.Run(element.children, null,true);
                // yield return new WaitForSeconds((float) delay());
            }

            element.isCompleted = true;
            yield return null;
        }
}