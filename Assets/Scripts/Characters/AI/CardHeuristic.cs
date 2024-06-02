using System;
using System.Collections.Generic;
using Effect;
using UnityEngine;

public abstract class CardHeuristic {
    /// <summary>
    /// Cómo de buena es la carta. 10 es el valor base
    /// </summary>
    public float value = 10;
}

/// <summary>
/// Para cartas con un solo target o targets fáciles de elegir
/// </summary>
public  class SimpleHeuristic:CardHeuristic{
    /// <summary>
    /// Indica si hay que usarlo en aliados o enemigos
    /// </summary>
    bool useOnTeammates;
}

