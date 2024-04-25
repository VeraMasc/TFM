using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Contiene la información que define a un personaje
/// </summary>
[Serializable]
public class EntityData{
    public string name;

    /// <summary>
    /// Estado de su decklist
    /// </summary>
    public DecklistState state;
    
    /// <summary>
    /// Decklist del personaje
    /// </summary>
    public DecklistDefinition decklist;

    /// <summary>
    /// Habilidades de los personajes
    /// </summary>
    public List<MyCardDefinition> abilities;
}