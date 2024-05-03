using CustomInspector;
using System;
using Unity;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using  CardHouse;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Gestiona la detección del targetting
/// </summary>
public class TargetDetector : MonoBehaviour
{
    public ITargetable actualTarget;

    /// <summary>
    /// Número de target dentro de los targets seleccionados. -1 si no está seleccionado
    /// </summary>
    public int targetIndex = -1;

    public bool isTarget => targetIndex != -1;

    void Start()
    {
        actualTarget = GetComponentInParent<ITargetable>();
    }


    void OnMouseDown()
    {
        if (!(EventSystem.current?.IsPointerOverGameObject() ?? false)){
            Debug.Log("Click Registered");
            if(!isTarget && isValid()){
                targetIndex = 0;
                Debug.Log($"Targeted {actualTarget} (Target Nº {targetIndex})", (UnityEngine.Object)actualTarget);
                
            }
            else if(isTarget){
                Debug.Log($"UnTargeted {actualTarget} (Target Nº {targetIndex})", (UnityEngine.Object)actualTarget);
                targetIndex = -1;
            }
        }
            

    }

    /// <summary>
    /// Calcula si es un target válido
    /// </summary>
    /// <returns></returns>
    public bool isValid(){
        return GameUI.singleton.possibleTargets?.Any((target)=> target == actualTarget) ?? false;
    }
}

