using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using Effect;
using UnityEngine;

/// <summary>
/// Clase base de la IA que controla entidades
/// </summary>
public class EntityAI : MonoBehaviour
{

    /// <summary>
    /// Entidad a controlar
    /// </summary>
    [SelfFill]
    public Entity entity;

    /// <summary>
    /// Comprueba entre todas las acciones disponibles cual es la mejor
    /// </summary>
    public virtual IActionable findBestAction(IEnumerable<IActionable> options){

        //TODO: Filtrar por heurística
        //Selección aleatoria
        return options.Random();
    }

    /// <summary>
    /// Ejecuta el turno de la IA
    /// </summary>
    public virtual IEnumerator doTurn(){
        Debug.Log("Doing Turn");
        var actionables = entity.getAllActionables();
        var filtered = actionables.Where( a => a.getSpeed()==SpeedTypes.Action 
                && (!(a?.getHeuristic() is ITimingHeuristic timing) || timing.isGoodTime()));

        var chosen = findBestAction(filtered);
        Debug.Log(chosen);
        if(chosen!=null){
            yield return entity.executeAction(chosen).Start(this);
        }
        else{//Pasar si no tiene nada que pueda hacer
            GameMode.current.passPriority(EntityTeam.enemy);
        }

    }

    /// <summary>
    /// Botón de test que fuerza el inicio del turno
    /// </summary>
    [NaughtyAttributes.Button(enabledMode: NaughtyAttributes.EButtonEnableMode.Playmode)]
    protected void startTurn(){
        StartCoroutine(doTurn());
    }

     /// <summary>
    /// Botón de test que muestra todos los accionables
    /// </summary>
    [NaughtyAttributes.Button(enabledMode: NaughtyAttributes.EButtonEnableMode.Playmode)]
    protected void showActionables(){
        var actionables = entity.getAllActionables();
        var text = string.Join("\n", actionables);
        Debug.Log($"{entity} Actionables:({actionables.Count})\n\n{text}\n\n", entity);
    }


    /// <summary>
    /// Ejecuta la reacción de la IA
    /// </summary>
    public virtual IEnumerator doReaction(){
        // Debug.Log("Doing Reaction");
        var actionables = entity.getAllActionables();
        var filtered = actionables
            .Where( a => (a?.getHeuristic() is IResponseHeuristic response) 
                && response.isGoodTime());//Es buen momento?

        var chosen = findBestAction(filtered);
        // Debug.Log(chosen);
        if(chosen!=null){
            yield return entity.executeAction(chosen).Start(this);
        }
       
    }

    /// <summary>
    /// Ejecuta la reacción de timing la IA
    /// </summary>
    public virtual IEnumerator doTimingReaction(){
        //Probabilidad de no reaccionar
        var combat = CombatController.singleton;
        if(Random.value >0.1 && !(combat?.followingTurn == entity && combat.currentPhase == CombatPhases.end))
        {
            yield break;
        }

        // Debug.Log("Doing Timing Reaction");
        var actionables = entity.getAllActionables();
        var filtered = actionables
            .Where( a => !(a?.getHeuristic() is ITimingHeuristic timing) //Es buen momento?
                || timing.isGoodTime());

        var chosen = findBestAction(filtered);
        Debug.Log(chosen);
        if(chosen!=null){
            yield return entity.executeAction(chosen).Start(this);
        }
        
    }

    /// <summary>
    /// Escoge los mejores targets de una lista de opciones
    /// </summary>
    /// <returns></returns>
    public virtual ITargetable[] chooseTargets(IEnumerable<ITargetable> options, ChoiceInfo info){
        var maxTargets = info is RangedChoiceInfo ranged? ranged.max : info.amount;
        if(maxTargets != -1){
            var heuristics = info.context?.heuristics
                .OfType<ITargetingHeuristic>();
            //Filtrar según heurística
            foreach(var heuristic in heuristics){
                options = heuristic.removeBadTargets(options,info.context);
            }
            var ret = options.TakeRandom(maxTargets);
            return ret.ToArray();
        }
        return options.ToArray();
    }

    /// <summary>
    /// Escoje los peores targets de una lista de opciones. Usado para cosas como descarte o similares
    /// </summary>
    /// <returns></returns>
    public virtual ITargetable[] rejectTargets(IEnumerable<ITargetable> options, ChoiceInfo info){
        
        
        if(info.amount != -1){
            var heuristics = info.context?.heuristics
                .OfType<ITargetingHeuristic>();
            //Filtrar según heurística
            foreach(var heuristic in heuristics){
                options = heuristic.keepBadTargets(options,info.context);
            }
            var ret = options.TakeRandom(info.amount);
            return ret.ToArray();
        }
        return options.ToArray();
    }

}

/// <summary>
/// Interfaz de todas las cosas que una IA puede escoger hacer
/// </summary>
public interface IActionable{
    public SpeedTypes getSpeed(){
        if(this is ActionCard action)
            return action.speedType;
        if (this is ActivatedAbility activated)
            return activated.speed;
        
        return SpeedTypes.Action;
    }

    public CardHeuristic getHeuristic(){
        if(this is ActionCard action){
            return (action.definition as ActionCardDefinition).heuristic;
        }
        if (this is ActivatedAbility activated){
            var card = activated.source;
            return ((card.data as ActionCard).definition as ActionCardDefinition).heuristic;
        }
        
        return null;
    }
}
