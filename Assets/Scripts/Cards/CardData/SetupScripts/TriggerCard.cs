using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;
using System;


/// <summary>
/// Gestiona la generación de pseudo cartas para la gestión de triggers
/// </summary>
public class TriggerCard : MyCardSetup
{
    /// <summary>
    /// Carta a partir de la que se genera el trigger
    /// </summary>
    public Card source;

    public override void Apply(CardDefinition data)
    {
        throw new Exception("Don't create triggers with Apply");
    }
}