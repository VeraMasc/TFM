using UnityEngine;
using System;

/// <summary>
/// Describe un efecto que permite al jugador elegir
/// </summary>
[Serializable]
public abstract class ChoiceDescriptor: EffectDescriptor 
{
    /// <summary>
    /// Nombre que identifica la decisión
    /// </summary>
    public string choiceName;
}