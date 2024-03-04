using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Elemento básico de todos los efectos del juego
/// </summary>
[Serializable]
public abstract class BaseEffect 
{
    /// <summary>
    /// Targets seleccionados para el efecto
    /// </summary>
    public List<GameObject> selectedTargets;

    /// <summary>
    /// Describes exactly what the effect does in order
    /// </summary>
    [SerializeReference, SubclassSelector]
    public List<EffectDescriptor> descriptors;


    //TODO: lista de targets pedidos por el efecto y su configuración
}
