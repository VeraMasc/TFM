using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;


/// <summary>
/// Contiene los counters de un objeto
/// </summary>
public class CounterHolder : MonoBehaviour
{

    public Transform counterList;
    public SerializableDictionary<string,int> counters = new();

    public List<CounterDisplay> displays = new();

    /// <summary>
    /// Indica si se ha cambiado desde la última vez que se actualizó
    /// </summary>
    public bool hasChanged;



    void Start()
    {
        var setup = GetComponent<MyCardSetup>();
        
        counterList = setup?.counterList ??  GetComponent<Entity>()?.counterList;
        
        displayCounters();
    }

    public void Update()
    {
        if (hasChanged)
            displayCounters();
    }

    /// <summary>
    /// Vacía por completo el holder
    /// </summary>
    public void clear(){
        counters.Clear();
        foreach(var disp in displays){
            if(disp!=null)
                Destroy(disp);
        }
        displays.Clear();
    }

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
        holder.hasChanged = true;
        return holder.counters;
    }

    /// <summary>
    /// Asigna un valor concreto a un tipo de counter
    /// </summary>
    public static void setCounter(ITargetable targetable, string key, int value){
        var dict = getCounterDict(targetable);
        Debug.Log(dict);
        if(!dict.TryAdd(key,value))
            dict[key] = value;

    }
    /// <summary>
    /// Asigna un valor concreto a un tipo de counter
    /// </summary>
    public static int addCounter(ITargetable targetable, string key, int increase=1){
        var dict = getCounterDict(targetable);
        dict.TryAdd(key,0); //Lo inicializa a 0 si no existe
        dict[key] += increase;
        var ret = dict[key];
        if(dict[key]<=0){
            dict.Remove(key);
        }
        return ret;
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

    /// <summary>
    /// Actualiza la representación de los contadores de la carta
    /// </summary>
    [NaughtyAttributes.Button]
    public void displayCounters(){
        hasChanged=false;

        if(!counterList)
            return;

        //Clear excess
        while(counters.Count < displays.Count){
            var removed = displays.Last();
            Destroy(removed.gameObject);
            displays.RemoveAt(displays.Count-1);
        }
        var i=0;
        var creator = GameController.singleton.creationManager;
        foreach(var (key,num) in counters){
            if(i>= displays.Count){ //Add displays as needed
                var instance = Instantiate(creator.counterDisplay, counterList);
                displays.Add(instance);
            }

            //Set displays
            var display = displays[i];
            display.setCounter(key,num);
            i++;
        }

    }


}
