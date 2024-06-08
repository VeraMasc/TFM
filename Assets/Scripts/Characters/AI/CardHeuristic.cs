using System;
using System.Collections.Generic;
using Effect;
using UnityEngine;

public abstract class CardHeuristic {
    /// <summary>
    /// Cómo de buena es la carta. 10 es el valor base
    /// </summary>
    public float quality = 10;

    public virtual void validate(){

    }
}

/// <summary>
/// Para cartas con un solo target o targets fáciles de elegir
/// </summary>
[Serializable]
public  class SimpleHeuristic:CardHeuristic{
    /// <summary>
    /// Indica si hay que usarlo en aliados o enemigos
    /// </summary>
    public bool useOnTeammates;
}

/// <summary>
/// Para cartas de ramp
/// </summary>
[Serializable]
public  class Ramp:CardHeuristic{
    
}

