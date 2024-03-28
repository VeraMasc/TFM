using System;
using UnityEngine;

[Serializable]
public class EffectDraw : EffectScript
{
    /// <summary>
    /// Quién roba las cartas
    /// </summary>
    [SerializeReference, SubclassSelector]
    EffectTargeter target;
    public int amount = 2;
}