using System;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Effect;
using UnityEngine;

public abstract class CardHeuristic {
    /// <summary>
    /// Cómo de buena es la carta. 10 es el valor base
    /// </summary>
    public float quality = 10;

    public virtual void validate(){

    }
}

/// <summary>
/// Para cartas con un solo target o targets fáciles de elegir
/// </summary>
[Serializable]
public  class SimpleHeuristic:CardHeuristic,ITargetingHeuristic{
    /// <summary>
    /// Indica si hay que usarlo en aliados o enemigos
    /// </summary>
    public bool useOnTeammates;

    public IEnumerable<ITargetable> removeBadTargets(IEnumerable<ITargetable> targets, Context context){
        var team = context.controller.team;
        var ret = targets
            .Where(t => !(t is Entity entity) || (entity.team != team ^ useOnTeammates ))
            .Where(t => !(t is Card card ) 
                    || (card.ownership?.controller?.team != team ^ useOnTeammates ));
            
        return ret.ToList();
    }
}

/// <summary>
/// Para cartas de ramp
/// </summary>
[Serializable]
public class Ramp : CardHeuristic, ITimingHeuristic
{
    public bool isGoodTime()
    {
        return true;
    }
}

/// <summary>
/// Heuristicas que cambian el momento en el que usar una carta
/// </summary>
public interface ITimingHeuristic{
    public bool isGoodTime();
}

/// <summary>
/// Heurística que define en respuesta a qué se debe usar una carta
/// </summary>
public interface IResponseHeuristic:ITimingHeuristic{
    
}

/// <summary>
/// Heuristicas que cambian el cómo se hace target
/// </summary>
public interface ITargetingHeuristic{

    /// <summary>
    /// Elimina los targets que nunca deberían ser escogidos por ser muy malos
    /// </summary>
    /// <param name="targets">Lista de targets</param>
    /// <returns></returns>
    public virtual IEnumerable<ITargetable> removeBadTargets(IEnumerable<ITargetable> targets,Context context){
        return targets;
    }

    /// <summary>
    /// Deja solo los targets malos. Se usa para selecciones inversas (en las que buscas escoger lo malo en vez de lo bueno)
    /// </summary>
    /// <param name="targets">Lista de targets</param>
    /// <returns></returns>
    public virtual IEnumerable<ITargetable> keepBadTargets(IEnumerable<ITargetable> targets,Context context){
        return targets;
    }
}

