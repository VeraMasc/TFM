using System;
using System.Collections.Generic;
using System.ComponentModel;
using CardHouse;
using UnityEngine;



/// <summary>
/// Clase contenedora de todas las modificaciones
/// </summary>
public class CardModifiers : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    public List<BaseModifier> modifiers = new();


    /// <summary>
    /// Añade un modificador concreto a una carta
    /// </summary>
    public static void addModifier(Card card, BaseModifier modifier, object initParams=null){
        //Get or create holder
        var holder = card.GetComponent<CardModifiers>();
        holder ??= card.gameObject.AddComponent<CardModifiers>();
        var newMod = modifier.clone(card);
        holder.modifiers.Add(newMod);
        newMod.initialize(initParams);
    }

    /// <summary>
    /// Elimina un modificador concreto de una carta
    /// </summary>
    /// <param name="card"></param>
    /// <param name="modifier">Referencia del modificador a eliminar</param>
    public static void removeModifier(Card card, BaseModifier modifier){
        //Get or create holder
        var holder = card.GetComponent<CardModifiers>();
        holder?.modifiers?.Remove(modifier);

        //Eliminar holder si queda vacío
        if(holder?.modifiers?.Count ==0){
            Destroy(holder);
        }
    }

    /// <summary>
    /// Devuelve una lista de los modifiers de la carta
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public static List<BaseModifier> getModifiers(Card card){
        var holder = card.GetComponent<CardModifiers>();
        return holder?.modifiers ?? new(); //Devolver lista vacía si no hay
    }
    

    private void OnValidate() {
        refreshModifiers();
    }

    public void refreshModifiers(){
        foreach(var mod in modifiers){
            mod.refresh();
        }
    }
}


/// <summary>
/// Clase madre de todos los modificadores
/// </summary>
[Serializable]
public abstract class BaseModifier
{
    /// <summary>
    /// Carta a la que se modifica
    /// </summary>
    [CustomInspector.ReadOnly]
    public Card modified;

    public virtual void initialize(object initParams){
        
    }

    /// <summary>
    /// Clona el modificador
    /// </summary>
    /// <param name="card">Carta a la que modificará la copia</param>
    /// <returns></returns>
    public virtual BaseModifier clone(Card newModified = null){
        var data = JsonUtility.ToJson(this);
        var ret = (BaseModifier) JsonUtility.FromJson(data,this.GetType());
        if(newModified != null)
            ret.modified = newModified;
        return ret;
    }

    /// <summary>
    /// Elimina el modificador de la carta que modifica
    /// </summary>
    public void removeSelf(){
        CardModifiers.removeModifier(modified, this);
    }

    public virtual void refresh(){
        
    }
}