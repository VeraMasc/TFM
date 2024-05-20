using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using UnityEngine;

/// <summary>
/// Clase madre de los gestores de modos de juego
/// </summary>
public abstract class GameMode : MonoBehaviour
{

    /// <summary>
    /// Personaje actualmente seleccionado para realizar sus acciones
    /// </summary>
    public Entity selectedPlayer;
    
    /// <summary>
    /// Stack de resolución de efectos
    /// </summary>
    public CardResolveOperator effectStack;

    /// <summary>
    /// Orden de prioridad de los equipos
    /// </summary>
    public List<EntityTeam> priorityOrder;

    /// <summary>
    /// Índice de la prioridad actual
    /// </summary>
    [SerializeField]
    protected int priorityIndex;

    /// <summary>
    /// Indica quién tiene la prioridad ahora mismo
    /// </summary>
    public EntityTeam currentPriority => priorityOrder[priorityIndex];

	///<summary>Recupera el modo de juego actual</summary>
	public static GameMode current
	{
		get 
		{
			return (CombatController.singleton as GameMode) ?? ExplorationController.singleton;
		}
	}

    /// <summary>
    /// devuelve si una entidad concreta "está en su turno" (puede realizar todo tipo de acciones)
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual bool isEntityTurn(Entity entity){
        //Por defecto, es turno de todos
        return true;
    }
    protected TriggerManager triggerManager => GameController.singleton.triggerManager;
    

    public void setSelectedPlayer(Entity player){
        var allEntities = GameController.singleton?.entities ?? new List<Entity>();

        foreach (var entity in allEntities){
            if(entity?.hand?.Strategy is HandLayout hand && hand.selected == true){
                hand.selected=false;
                entity.hand.ApplyStrategy();
            }
        }

        selectedPlayer = player;

        if(player?.hand?.Strategy is HandLayout playerHand){
            playerHand.selected=true;
            player.hand.ApplyStrategy();
        }
    }

    /// <summary>
    /// Activa la acción de paso de fase/turno o resolución de cartas según sea necesario
    /// </summary>
    public void passPriority(EntityTeam team){
        var stack = CardResolveOperator.singleton;

        if(team == currentPriority){ //pasar solo si tiene prioridad
            priorityIndex++;
        }
        
        if(priorityIndex >= priorityOrder.Count){ //Si ambos pasan
            priorityIndex=0;//Reset

            if(!stack.isEmpty){ //Primero resolver cosas del stack
                if(!stack.precalculating){//No resolver si esá precalculando
                    stack.startResolve = true;
                }
            }
            else{
                nextPhase();
            }
        }
        if(this is CombatController combat){
            combat.aiDirector.onPriorityChange();
        }
        
    }
    
    public void playerPassPriority() => passPriority(EntityTeam.player);

    /// <summary>
    /// Pasa a la siguiente fase
    /// </summary>
    public virtual void nextPhase(){

    }

    /// <summary>
	/// Comprueba el estado actual de las cosas y realiza las acciones de estado correspondientes
	/// </summary>
	public virtual void checkState(){
		
	}

    public virtual void healthCheck(){
		foreach(var entity in GameController.singleton.entities){
            if(entity.health<=0){
                StartCoroutine(entity.kill());
            }
        }
	}

    public virtual void getPriorityOrder(){
        priorityOrder = new List<EntityTeam>(){EntityTeam.player};
    }
}
