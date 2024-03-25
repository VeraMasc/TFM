using UnityEngine;
using System;
using Unity.VisualScripting;



/// <summary>
/// Roba cartas
/// </summary>
[UnitCategory("CardEffects")]
public class DrawCards : EffectDescriptor
{
    [DoNotSerialize]
    public ValueInput amount;
    protected override void PortDefinition(){
        ValueInput<int>("amount", 1);
        Requirement(amount, inputTrigger);
        Succession(inputTrigger, outputTrigger); 
    }


    protected override Func<Flow,ControlOutput> LogicDefinition(){
        return (flow) => {
            
            var n = flow.GetValue<int>(amount);
            Debug.Log($"Draw {n} Cards");
            return outputTrigger; 
        };
    }
}
