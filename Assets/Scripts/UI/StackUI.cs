using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Gestor de la interfaz del stack
/// </summary>
public class StackUI : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public SplineAnchorFollow sourceSplineOrigin;

    /// <summary>
    /// 
    /// </summary>
    public GameObject sourceSpline;

    /// <summary>
    /// Stack
    /// </summary>
    [SelfFill(true)]
    public CardResolveOperator stack;

    /// <summary>
    /// Actualiza el estado de los elementos de la interfaz
    /// </summary>
    public void refresh(){
        setSourceSpline();
    }

    /// <summary>
    /// Configura la spline de origen
    /// </summary>
    public void setSourceSpline(){
        var card = stack.topCard;
        
        if(card?.data is TriggerCard trigger){//Si es un trigger
            //TODO: Make sourcespline follow source card
            sourceSplineOrigin.target = trigger.source.transform;
            sourceSplineOrigin.followTarget();
            sourceSpline.SetActive(true);

            return;
        }
        sourceSpline.SetActive(false);
    }
}
