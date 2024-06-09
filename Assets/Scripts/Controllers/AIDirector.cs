using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Gestiona la actividad de todas las IAs del juego
/// </summary>
public class AIDirector : MonoBehaviour
{
    /// <summary>
    /// Desactiva el director, hace que las IAs se mantengan en stand-by
    /// </summary>
    public bool disabled;

    public void onPriorityChange(){
        if(GameMode.current.currentPriority == EntityTeam.enemy){
            Debug.Log("AI Has Priority!!!");
            if(disabled){
                GameMode.current.passPriority(EntityTeam.enemy);
            }
            else{
                AITakeAction();
            }
            
        }
    }

    /// <summary>
    /// Hace que una de las IAs tome una acción según corresponda
    /// </summary>
    [NaughtyAttributes.Button]
    public void AITakeAction(){
        var combat = CombatController.singleton;
        if(!CardResolveOperator.singleton.isEmpty){
             Debug.Log("AI's Response");
        }else if(combat.currentTurn.AI && combat.currentPhase == CombatPhases.main){ //Si es turno de la IA
            Debug.Log("AI's Main");
        }
        else {
            Debug.Log("AI's Timing Response");
        }
        
    }
    
    [NaughtyAttributes.Button]
    public void forcePass(){
        GameMode.current.passPriority(EntityTeam.enemy);
    }
}
