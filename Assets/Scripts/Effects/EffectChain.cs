using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Identifica una cadena de efectos
/// </summary>
[Serializable]
public class EffectChain
{
    [SerializeReference,SubclassSelector]
    public List<EffectScript> list;
}
/// <summary>
/// Indica cada pieza de la cadena de efectos
/// </summary>
[Serializable]
public class EffectChainLink
{
    [SerializeReference,SubclassSelector]
    public EffectScript effect;

    [SerializeReference] //Necesario para que serialice bien
    public EffectChainLink next;
}