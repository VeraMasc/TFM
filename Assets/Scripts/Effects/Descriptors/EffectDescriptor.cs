using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Clase madre para todas las cosas que puede hacer un efecto
/// </summary>
public abstract class EffectDescriptor : Unit
{
    [DoNotSerialize] // No need to serialize ports.
    public ControlInput inputTrigger; //Adding the ControlInput port variable

    [DoNotSerialize] // No need to serialize ports.
    public ControlOutput outputTrigger;//Adding the ControlOutput port variable.

    /// <summary>
    /// The method to set what our node will be doing.
    /// </summary>
    protected override void Definition()
    {
        //Making the ControlInput port visible, setting its key and running the anonymous action method to pass the flow to the outputTrigger port.
        inputTrigger = ControlInput("inputTrigger", LogicDefinition());
        //Making the ControlOutput port visible and setting its key.
        outputTrigger = ControlOutput("outputTrigger");
        Succession(inputTrigger, outputTrigger); 
        PortDefinition();
    }
    
    /// <summary>
    /// Sobreescribir para añadir puertos nuevos
    /// </summary>
    protected virtual void PortDefinition(){

    }
    /// <summary>
    /// Sobreescribir para añadir lógica nueva
    /// </summary>
    protected virtual Func<Flow,ControlOutput> LogicDefinition(){
        return (flow) => { 
            return outputTrigger; 
        };
    }
}