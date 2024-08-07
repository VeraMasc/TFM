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

    /// <summary>
    /// Indica que es un trigger creado activamente por un jugador
    /// </summary>
    public bool isActiveTrigger;

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
        gameObject.name +=" Trigger";
        effects = new BaseCardEffects
        {
            baseEffect = triggered.clone() //Asigna el trigger como efecto base
        };
    }

    /// <summary>
    /// Permite cambiar el texto de los triggers
    /// </summary>
    /// <param name="newText"></param>
    public void changeText(string newText){
        Debug.Log($"Text changed to: {newText}");
        cardText = newText;
        cardTextBox.text = cardText;
    }
}