using System;
using System.Collections.Generic;
using Effect;
using UnityEngine;

/// <summary>
/// Da información extra a la IA sobre el tipo de decisión que está tomando
/// </summary>
public class ChoiceInfo 
{
    /// <summary>
    /// Contexto de la carta que lo ejecuta (si existe)
    /// </summary>
    public Context context;

    /// <summary>
    /// Cantidad de elementos que ha de escoger. -1 si si no hay cantidad definida
    /// </summary>
    public int amount;

    /// <summary>
    /// Indica si la acción se puede "cancelar" o si es obligatoria
    /// </summary>
    public bool cancellable;

    /// <summary>
    /// Función de validación
    /// </summary>
    public Func<IEnumerable<ITargetable>,bool> validator;
}

/// <summary>
/// Da información sobre las decisiones en las que hay un rango de valores válidos
/// </summary>
public class RangedChoiceInfo:ChoiceInfo{
    public int min;
    public int max;
}