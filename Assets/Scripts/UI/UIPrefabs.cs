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
    

    [HorizontalLine(3,message = "User Inputs")]
    public NumericInput numericInput;

    public YesNoInput yesNoInput;

    public YesNoInput confirmationInput;

    public CardSelectInput cardSelectInput;

    public EventCallInput GameOverInput;

    public EventCallInput PauseInput;

    [HorizontalLine(3,message = "Components")]
    /// <summary>
    /// Prefab de la barra de scroll
    /// </summary>
    public GameObject scrollbar;

    public GameObject clickBlocker;

    public CardSelectOption cardSelectOption;

    public CardSelectOption textSelectOption;

    [HorizontalLine(3,message = "Indicators")]
    public SpriteRenderer targettingIndicator;

}
