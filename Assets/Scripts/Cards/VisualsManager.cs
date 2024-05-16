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
                foreach (Transform child in element.transform)
                {
                    child.gameObject.SetActive(!hide);
                }
            }
        isHidden = hide;
        }
        
    }

    [NaughtyAttributes.Button]
    public void toggleVisuals(){
        hideVisuals(!isHidden);
    }

    public void cloneFace(Transform newParent){

        //Copy face
        var front = transform.Find("Front");
        if(!front)
            return;
        var newFront = Instantiate(front,newParent);
        newFront.localScale = new Vector3(1,0.8f,1);
        newFront.gameObject.SetActiveRecursively(true);


        //Copy outline
        var outline = transform.Find("Outline");
        if(!outline)
            return;
        var newOutline = Instantiate(outline,newParent);
        newOutline.localScale = new Vector3(1,0.8f,1);

    }
}
