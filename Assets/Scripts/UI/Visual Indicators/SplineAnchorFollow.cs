using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Hace que una spline siga a otro objeto
/// </summary>
public class SplineAnchorFollow : MonoBehaviour
{
    /// <summary>
    /// Objeto a seguir
    /// </summary>
    public Transform target;

    public Vector3 offset;
    
    void LateUpdate()
    {
        followTarget();
    }

    [Button]
    public void followTarget(){
        if(target != null){
            transform.position = target.position + offset;
        }
    }
}
