using System;
using System.Linq;
using CardHouse;
using UnityEngine;



/// <summary>
/// Fuerza a las cartas a ir a una zona concreta
/// </summary>
[Serializable]
public class ForceZoneModifier : BaseModifier
{
    /// <summary>
    /// Indica que el efecto se elimina después de que se produzca
    /// </summary>
    public bool singleUse;

    /// <summary>
    /// Zona a la que enviarlas
    /// </summary>
    public GroupName sendTo;

    /// <summary>
    /// Activa el forzado de zona
    /// </summary>
    /// <returns>Devuelve si ha tenido éxito</returns>
    public bool activate(){
        var ownership = modified.GetComponent<CardOwnership>();
        var group = GameObject.FindObjectsOfType<GroupZone>()
                .Where(group => group.zone == sendTo && group.owner == ownership?.owner)
                .FirstOrDefault()
                ?.GetComponent<CardGroup>();
        if (group== null)
            return false;
        
        group.Mount(modified);
        if(singleUse)
            removeSelf();
        return true;
    }

}