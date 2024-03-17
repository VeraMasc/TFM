using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Describe ciertos stats de una carta
/// </summary>
[RequireComponent(typeof(Card))]
public class CardStats : MonoBehaviour
{
    /// <summary>
    /// Coste total de la carta
    /// </summary>
    public int manaValue;

    /// <summary>
    /// Contiene otros valors numéricos
    /// </summary>
    public Dictionary<string,int> extraValues;

    public Dictionary<string,GameObject> targets;
}