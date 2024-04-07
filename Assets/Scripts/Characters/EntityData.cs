using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Contiene la información que define a un personaje
/// </summary>
public class EntityData{
    public string name;

    /// <summary>
    /// Estado de su decklist
    /// </summary>
    public DecklistDefinition state;
    
    /// <summary>
    /// Decklist del personaje
    /// </summary>
    public DecklistDefinition decklist;

    /// <summary>
    /// Habilidades de los personajes
    /// </summary>
    public List<MyCardDefinition> abilities;
}