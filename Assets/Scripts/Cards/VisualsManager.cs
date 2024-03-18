using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Se encarga de gestionar los elementos visuales de las cartas
/// </summary>
public class VisualsManager : MonoBehaviour
{
    public GameObject[] visualElements;

    [SelfFill]
    public Homing homing;
    [SelfFill]
    public Scaling scaling;

    [SelfFill]
    public Turning turning;

    [ReadOnly]
    public bool isHidden;


    public void hideVisuals(bool hide){
        if(isHidden != hide){
            foreach(var element in visualElements){
                element.SetActive(!hide);
            }
        isHidden = hide;
        }
        
    }
}
