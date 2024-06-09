using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Gestiona el combate del juego
/// </summary>
public class CombatController : GameMode
{
	/// <summary>
    /// Cola de turnos
    /// </summary>
    public List<Entity> turnOrder;

	private List<Entity> nextTurnOrder;

	public int roundCount;

	public Entity currentTurn => turnOrder?.LastOrDefault();

	/// <summary>
	/// Fase actual del combate
	/// </summary>
	public CombatPhases currentPhase;

	/// <summary>
	/// Corrutina activa de la fase actual, no se pueden activar otras corrutinas de fase hasta que acabe
	/// </summary>
	public Coroutine phaseCoroutine;

	/// <summary>
	/// Gestor de todas las IAs
	/// </summary>
	[SelfFill]
	public AIDirector aiDirector;

	/// <summary>
	/// Campo de juego
	/// </summary>
	public CardGroup board;

    private static CombatController _singleton;
	///<summary>Controller Singleton</summary>
	public static CombatController singleton
	{
		get 
		{
			if (_singleton == null)
			{
				_singleton = FindObjectOfType<CombatController>(); //Para cuando el maldito hotreload me pierde la referencia
			}
			return _singleton;
		}
	}

    void Awake()
    {
        _singleton =this;
    }

	void Start()
	{
		StartCoroutine(beginCombat());
	}

	/// <summary>
	/// Gestiona el inicio del combate
	/// </summary>
	/// <returns></returns>
	public IEnumerator beginCombat(){
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.2f);
		var entities = GameController.singleton?.entities ?? new List<Entity>();
		
        //Initial card draw
        
		yield return UCoroutine.YieldInParallel(
				entities.Select(entity => UCoroutine.Yield(entity.draw(4)))
			.ToArray()).Start(this);
		
		
		yield return new WaitForSeconds(0.5f);
		generateTurnOrder();
		nextTurn();
		yield return triggerManager.onStartCombat.invoke().Start(this);
		executePhase();
		
    }

	public override bool isEntityTurn(Entity entity){
        return entity == currentTurn;
    }

	public override bool isSpeedValid(Entity entity, SpeedTypes speed){
        if(speed == SpeedTypes.Reaction){
			return true;
		}

		return GameMode.current.isEntityTurn(entity) && currentPhase == CombatPhases.main && CardResolveOperator.singleton.isEmpty;
		
    }

	/// <summary>
    /// Regenera la cola de turnos
    /// </summary>
    [NaughtyAttributes.Button]
    public void generateTurnOrder(){
        var entities = GameController.singleton?.entities ?? new List<Entity>();
        entities = entities?.ToList();
        nextTurnOrder = new List<Entity>();

        while (entities.Count > 0)
        {
            var chosenIndex = UnityEngine.Random.Range(0, entities.Count);
            nextTurnOrder.Add(entities[chosenIndex]);
            entities.RemoveAt(chosenIndex);
        }
    }
	public override void nextPhase(){
        currentPhase = (CombatPhases)(((int)currentPhase+1) % CombatPhases.GetNames(typeof(CombatPhases)).Length);
		if(currentPhase == CombatPhases.setup){
			nextTurn();
		}
		executePhase();
    }


	/// <summary>
	/// Ejecuta la fase actual
	/// </summary>
	public void executePhase(){
		if(phaseCoroutine != null){
			Debug.LogWarning("A phase coroutine is already active! Wait for it to finish before starting another");
		}

		
		Debug.Log($"Executing phase{currentPhase}");
		
		if(currentPhase == CombatPhases.setup){
			phaseCoroutine =StartCoroutine(setupPhase());
		}
		else if(currentPhase == CombatPhases.start){
			phaseCoroutine =StartCoroutine(startPhase());
		}
		else if(currentPhase == CombatPhases.main){

		}
		else if(currentPhase == CombatPhases.end){
			phaseCoroutine =StartCoroutine(endPhase());
		}
		else if(currentPhase == CombatPhases.cleanup){
			phaseCoroutine =StartCoroutine(cleanupPhase());
		}
		
	}

	/// <summary>
	/// Pasa al siguiente turno
	/// </summary>
	public void nextTurn(){
		var remaining = turnOrder.Count;
		if(remaining >1){
			turnOrder.RemoveAt( remaining-1);
		}
		
		if(remaining==2){
			generateTurnOrder();
		}else if(remaining<=1){
			turnOrder = nextTurnOrder;
			nextTurnOrder =null;
			roundCount++;
		}
		getPriorityOrder();
	}

	/// <summary>
	/// La entidad actual usa su redraw gratuito
	/// </summary>
	public void freeRedraw(){
		var entity = selectedPlayer;
		if(entity==null || currentTurn != entity && !disableTimingRestrictions)
			return;

		StartCoroutine(Effect.ReDraw.basicRedraw(1,entity));
	}
	
	/// <summary>
	/// Se encarga de ejecutar todas las partes del inicio de un turno
	/// </summary>
	/// <returns></returns>
	public IEnumerator setupPhase(){
		var z = transform.position.z;
		transform.position = (Vector2) currentTurn.targeterTransform.transform.position;
		transform.position += Vector3.forward * z;

		//Select active entity hand if possible
		currentTurn.trySelectPlayer();
		currentTurn.mana.loseAll();
		yield return StartCoroutine(triggerManager.beforeBeginTurn.invoke());
		yield return StartCoroutine(currentTurn.draw(1));
		yield return CardResolveOperator.singleton.waitTillEmpty;
		//End coroutine
		phaseCoroutine = null;
		nextPhase();
	}

	/// <summary>
	/// Se encarga de ejecutar los eventos de inicio de turno
	/// </summary>
	/// <returns></returns>
	public IEnumerator startPhase(){
		yield return StartCoroutine(triggerManager.onBeginTurn.invoke());
		//End coroutine
		phaseCoroutine = null;
	}

	/// <summary>
	/// Se encarga de ejecutar los eventos de final de turno
	/// </summary>
	public IEnumerator endPhase(){
		yield return StartCoroutine(triggerManager.onEndTurn.invoke());
		//End coroutine
		phaseCoroutine = null;
	}


	/// <summary>
	/// Se encarga de ejecutar todas las partes del final de un turno
	/// </summary>
	/// <returns></returns>
	public IEnumerator cleanupPhase(){
		yield return StartCoroutine(currentTurn.enforceHandSize());
		yield return CardResolveOperator.singleton.waitTillEmpty;
		//End coroutine
		phaseCoroutine = null;
		nextPhase();
		
	}


	public override void getPriorityOrder(){
        priorityOrder = currentTurn.team == EntityTeam.player?
			new List<EntityTeam>(){EntityTeam.player,EntityTeam.enemy}:
			new List<EntityTeam>(){EntityTeam.enemy,EntityTeam.player};
		priorityIndex = 0;
		aiDirector.onPriorityChange();
    }

	
	public override void getResponsePriority(EntityTeam inResponseTo){
        priorityOrder = inResponseTo == EntityTeam.player?
			new List<EntityTeam>(){EntityTeam.player,EntityTeam.enemy}:
			new List<EntityTeam>(){EntityTeam.enemy,EntityTeam.player};
		priorityIndex = 0;
		aiDirector.onPriorityChange();
    }
}


public enum CombatPhases {
	/// <summary>
	/// Roba carta y añade el maná. No se puede responder
	/// </summary>
	setup,
	/// <summary>
	/// Llama a los efectos de inicio de turno
	/// </summary>
	start,
	/// <summary>
	/// El turno del personaje como tal
	/// </summary>
	main,
	/// <summary>
	/// Llama a los efectos de final de turno, descarte por handsize y permite responder a pasar de turno
	/// </summary>
	end,
	/// <summary>
	/// Se encarga de limpiar los efectos temporales. No se puede responder
	/// </summary>
	cleanup,

}