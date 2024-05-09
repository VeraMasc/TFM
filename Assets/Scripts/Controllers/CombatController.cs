using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
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

	public Entity currentTurn => turnOrder?.LastOrDefault();

	/// <summary>
	/// Fase actual del combate
	/// </summary>
	public CombatPhases currentPhase;



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
				entities.Select(entity => entity.draw(4))
			.ToArray());
		generateTurnOrder();
        yield break;
    }

	public override bool isEntityTurn(Entity entity){
        var first = turnOrder.FirstOrDefault();
        return first == entity;
    }

	/// <summary>
    /// Regenera la cola de turnos
    /// </summary>
    [NaughtyAttributes.Button]
    public void generateTurnOrder(){
        var entities = GameController.singleton?.entities ?? new List<Entity>();
        entities = entities?.ToList();
        turnOrder = new List<Entity>();

        while (entities.Count > 0)
        {
            var chosenIndex = UnityEngine.Random.Range(0, entities.Count);
            turnOrder.Add(entities[chosenIndex]);
            entities.RemoveAt(chosenIndex);
        }
    }
	public override void nextPhase(){
        currentPhase = (CombatPhases)(((int)currentPhase+1) % (int)CombatPhases.cleanup); 
    }

	/// <summary>
	/// La entidad actual usa su redraw gratuito
	/// </summary>
	public void freeRedraw(){
		var entity = currentTurn;
		if(entity==null)
			return;

		StartCoroutine(Effect.ReDraw.basicRedraw(1,entity));
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