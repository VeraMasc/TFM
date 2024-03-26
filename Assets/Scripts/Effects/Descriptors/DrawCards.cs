using UnityEngine;
using System;
using Unity.VisualScripting;



/// <summary>
/// Nodo de prueba para visual scripts
/// </summary>
[UnitCategory("CardEffects")]
public class DrawCards : EffectDescriptor
{
    [DoNotSerialize]
    public ValueInput amount;
    protected override void PortDefinition(){
        amount = ValueInput<int>("amount", 1);
        Requirement(amount, inputTrigger);
    }


    protected override Func<Flow,ControlOutput> LogicDefinition(){
        return (flow) => {
            
            var n = flow.GetValue<int>(amount);
            Debug.Log($"Draw {n} Cards");
            return outputTrigger; 
        };
    }
}
