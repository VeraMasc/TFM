using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;


/// <summary>
/// Contiene los counters de un objeto
/// </summary>
public class CounterHolder : MonoBehaviour
{
    public SerializableDictionary<string,int> counters = new();

    /// <summary>
    /// Obtiene el holder de los contadores del objeto (si existe)
    /// </summary>
    /// <param name="targetable">Objeto en cuestión</param>
    public static CounterHolder getHolder(ITargetable targetable){
        //TODO: optimize counter retrieval
        return targetable.GetComponent<CounterHolder>();
    }

    /// <summary>
    /// Devuelve el diccionario con los counters del objeto (si no existe lo crea)
    /// </summary>
    public static SerializableDictionary<string,int> getCounterDict(ITargetable targetable){
        var holder = getHolder(targetable);
        holder ??= (targetable as Component)?.gameObject?.AddComponent<CounterHolder>();
        return holder.counters;
    }

    /// <summary>
    /// Asigna un valor concreto a un tipo de counter
    /// </summary>
    public static void setCounter(ITargetable targetable, string key, int value){
        var dict = getCounterDict(targetable);
        dict[key] = value;

    }
    /// <summary>
    /// Asigna un valor concreto a un tipo de counter
    /// </summary>
    public static void addCounter(ITargetable targetable, string key, int increase=1){
        var dict = getCounterDict(targetable);
        dict.TryAdd(key,0); //Lo inicializa a 0 si no existe
        dict[key] += increase;
    }

    /// <summary>
    /// Devuelve el número de counters de un tipo. Si no existe, da 0; 
    /// </summary>
    public static int getCounter(ITargetable targetable, string key){
        var dict = getHolder(targetable)?.counters; //No crear holder si no existe

        if(dict != null && dict.TryGetValue(key, out int value)){
            return value;
        }
        return 0;
    }
}
