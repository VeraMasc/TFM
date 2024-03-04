using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Efecto permite escoger entre varias opciones
/// </summary>
public class ModalEffect<T> : BaseEffect where T :BaseEffect
{
    /// <summary>
    /// Opciones entre las que escoger
    /// </summary>
    public List<T> options;

    /// <summary>
    /// Númereo máximo de opciones a escoger (-1 == sin límites)
    /// </summary>
    public int maxChosen =1;

    /// <summary>
    /// Númereo mínimo de opciones a escoger  (0 == sin límites)
    /// </summary>
    public int minChosen =1;
}
