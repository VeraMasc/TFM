using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Enum de los tipos que pueden tener las habitaciones (aparte de "Room")
/// </summary>
public enum RoomTypes{
    /// <summary>
    /// Solo hay una habitación de este tipo en la mazmorra
    /// </summary>
    legendary,
}

/// <summary>
/// Añade métodos de extensión a los sets de tipos
/// </summary>
public static class RoomTypeExtension{
    public static string[] getSupertypes(this SerializableSet<RoomTypes> roomTypes){
        throw new NotImplementedException();
    }

    public static string[] getSecondaryTypes(this SerializableSet<RoomTypes> roomTypes){
        throw new NotImplementedException();
    }
}