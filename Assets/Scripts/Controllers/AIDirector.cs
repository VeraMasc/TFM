using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using CustomInspector;
using DTT.Utils.Extensions;
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
            // Debug.Log("AI Has Priority!!!");
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
        var AIs = getAIs().ToList();
        if(!CardResolveOperator.singleton.isEmpty){
            Debug.Log("AI's Reaction");

            AIs.Shuffle();
            UCoroutine.YieldInSequence(AIs.Select(ai => ai.doReaction()).ToArray())
                .Then(()=> GameMode.current.passPriority(EntityTeam.enemy))
                .Start(this);
           

        }else if(combat.currentTurn.AI && combat.currentPhase == CombatPhases.main){ //Si es turno de la IA
            Debug.Log("AI's Main");
            combat.currentTurn.AI.doTurn().Start(combat.currentTurn.AI);
        }
        else {
            Debug.Log("AI's Timing Reaction");
            AIs.Shuffle();
            UCoroutine.YieldInSequence(AIs.Select(ai => ai.doTimingReaction()).ToArray())
                .Then(()=> GameMode.current.passPriority(EntityTeam.enemy))
                .Start(this);
        }
        
    }

    [NaughtyAttributes.Button]
    public void forcePass(){
        GameMode.current.passPriority(EntityTeam.enemy);
    }

    /// <summary>
    /// Recupera todas las IAs de la escena
    /// </summary>
    /// <returns></returns>
    public IEnumerable<EntityAI> getAIs(){
        return GameController.singleton.entities
            .Where( e => e.AI !=null)
            .Select(e => e.AI);
    }
}
