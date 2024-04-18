using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Tipo de input en el que el resultado es un número
/// </summary>
public class NumericInput : PlayerInputBase
{
    /// <summary>
    /// Controlador de la interfaz del selector
    /// </summary>
    public NumberSelectorUI selector;

    public override void confirmChoice(){
        inputValue= selector.currentValue; 
        isFinished=true;
    }
}
