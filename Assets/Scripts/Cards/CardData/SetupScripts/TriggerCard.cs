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

    
    /// <summary>
    /// Configura la carta de trigger en base a otra
    /// </summary>
    /// <param name="card">Carta de origen del efecto</param>
    /// <param name="triggered">Efecto que ha sido desencadenado</param>
    public virtual void ApplyTrigger(Card card, EffectChain triggered)
    {
        source = card;
        var data = card.data as MyCardSetup;

        //Clona los valores básicos de la carta de origen
        base.Apply(data.definition);

        effects = new BaseCardEffects
        {
            baseEffect = triggered.clone() //Asigna el trigger como efecto base
        };

    }
}