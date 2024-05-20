using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class AIDirector : MonoBehaviour
{

    public void onPriorityChange(){
        if(GameMode.current.currentPriority == EntityTeam.enemy){
            Debug.Log("AI Has Priority!!!");
            GameMode.current.passPriority(EntityTeam.enemy);
        }
    }
}
