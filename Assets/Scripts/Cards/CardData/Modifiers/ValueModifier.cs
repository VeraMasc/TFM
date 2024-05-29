using System;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Effect;
using Effect.Duration;
using UnityEngine;



/// <summary>
/// Añade un valor persistente a una carta en cuestión
/// </summary>
[Serializable]
public class ValueModifier : AbilityModifier
{

    public string id;
    public object value;

}

