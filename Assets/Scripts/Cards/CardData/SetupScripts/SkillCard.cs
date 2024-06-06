using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;


/// <summary>
/// Se encarga de la generación de las cartas de acción
/// </summary>
public class SkillCard : MyCardSetup
{
    
    public int level;
    public void applyLevel(int level){
        this.level = level;
        cardText = SkillCardDefinition.parseCardLevel(definition.parsedText,level);
        effects.setContext(GetComponent<Card>());
        refreshValues();
    }


    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        base.Apply(data);
        if (data is SkillCardDefinition skill)
        {
            effects = skill.effects;
            GameController.singleton.creationManager
                .replaceCardTypeline(GetComponent<Card>());
            //Effects
            effects = skill.effects;
            effects = skill.effects.cloneAll();
            effects.setContext(GetComponent<Card>());
        }
        else {
            Debug.LogError($"Wrong Definition for Skill Card: {data?.GetType()?.Name}",data);
        }
        refreshValues();
    }

    
    [Button]
    public void refreshValues(){
        cardTextBox.text = cardText;
    }

    
}

