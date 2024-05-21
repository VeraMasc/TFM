using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class LINQExtensions
{
    public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> data, int amount, bool allowRepetitions=false)
    {
        var list = data.ToList();
        var ret = new List<T>();

        for(var i =0; i<amount; i++){
            int index = UnityEngine.Random.Range(0,list.Count());
            ret.Add(list.ElementAt(index));

            if(!allowRepetitions){//Quitar elementos ya usados
                list.RemoveAt(index);
            }
        }

        return ret;
        
        
    }

    /// <summary>
    /// Devuelve un elemento al azar
    /// </summary>
    public static T Random<T>(this IEnumerable<T> data)
    {
        int index = UnityEngine.Random.Range(0, data.Count());
        return data.ElementAt(index);
    }
}