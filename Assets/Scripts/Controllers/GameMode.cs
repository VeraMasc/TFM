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

    /// <summary>
    /// Indica si una velocidad de acción es válida para una entidad
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual bool isSpeedValid(Entity entity, SpeedTypes speed){
        if(speed == SpeedTypes.Reaction){
			return true;
		}

		return GameMode.current.isEntityTurn(entity) && CardResolveOperator.singleton.isEmpty;
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

        if(GameUI.singleton?.activeUserInput) //No pasar si el usuario ha de introducir valores
            return;

        if(team == currentPriority){ //pasar solo si tiene prioridad
            priorityIndex++;
        }
        
        if(priorityIndex >= priorityOrder.Count){ //Si ambos pasan

            if(!stack.isEmpty){ //Primero resolver cosas del stack
                if(!stack.precalculating){//No resolver si esá precalculando
                    stack.startResolve = true;
                }
                getPriorityOrder();
            }
            else{
                getPriorityOrder();
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
		healthCheck();
	}

    public virtual void healthCheck(){
		foreach(var entity in GameController.singleton.entities){
            if(entity.health<=0){
                StartCoroutine(entity.kill());
            }
        }
	}

    /// <summary>
    /// Obtiene la prioridad base en un turno
    /// </summary>
    public virtual void getPriorityOrder(){
        priorityIndex = 0;
        priorityOrder = new List<EntityTeam>(){EntityTeam.player};
        
    }

    /// <summary>
    /// Obtiene la prioridad de respuesta a una acción
    /// </summary>
    /// <param name="inResponseTo">jugador que ha realizado la acción</param>
    public virtual void getResponsePriority(EntityTeam inResponseTo){
        priorityIndex = 0;
        priorityOrder = new List<EntityTeam>(){EntityTeam.player};
        
    }
}
