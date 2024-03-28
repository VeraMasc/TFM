using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Identifica una cadena de efectos
/// </summary>
[Serializable]
public class EffectChain 
{
    [SerializeReference,SubclassSelector]
    public EffectScript effect;

    [SerializeReference] //Necesario para que serialice bien
    public EffectChain next;
}
