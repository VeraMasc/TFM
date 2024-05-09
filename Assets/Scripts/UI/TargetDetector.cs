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
                GameUI.singleton.chosenTargets.Add(actualTarget);
                putTargetMarker(actualTarget,-1);
                
            }
            else if(isTarget){
                Debug.Log($"UnTargeted {actualTarget} (Target Nº {targetIndex})", (UnityEngine.Object)actualTarget);
                targetIndex = -1;
                GameUI.singleton.chosenTargets.Remove(actualTarget);
                clearTargetMarkers(actualTarget);
            }
        }
            

    }

    /// <summary>
    /// Resetea los valores internos del detector (RECUERDA ELIMINAR EL TARGET DE GameUI !!!)
    /// </summary>
    public void resetTargeting(){
        targetIndex =-1;
    }

    /// <summary>
    /// Calcula si es un target válido
    /// </summary>
    /// <returns></returns>
    public bool isValid(){
        return GameUI.singleton.possibleTargets?.Any((target)=> target == actualTarget) ?? false;
    }

    /// <summary>
    /// Pone un target marker sobre el targetable especificado
    /// </summary>
    /// <param name="targetable"></param>
    /// <param name="number"></param>
    public static void putTargetMarker(ITargetable targetable, int number){
        var prefab = GameUI.singleton.prefabs.targettingIndicator;
        var instance = Instantiate(prefab, targetable.targeterTransform);
        
        if(instance.transform.lossyScale.x >4){
            var transform =instance.transform;
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3 (4/transform.lossyScale.x, 4/transform.lossyScale.y, 4/transform.lossyScale.z);
        }
        GameUI.singleton.markedTargets.Add(targetable);
    }

    public static void clearTargetMarkers(ITargetable targetable){
        foreach(Transform child in targetable.targeterTransform){
            Destroy(child.gameObject);
        }
    }
}

