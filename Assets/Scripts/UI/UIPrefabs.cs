using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Guarda los prefabs de la interfaz
/// </summary>
[CreateAssetMenu(menuName = "TFM/UIPrefabs")]
public class UIPrefabs : ScriptableObject
{
    /// <summary>
    /// Prefab de la barra de scroll
    /// </summary>
    public GameObject scrollbar;

    [HorizontalLine(3,message = "User Inputs")]
    public NumericInput numericInput;

    public YesNoInput yesNoInput;

}
