using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // //Initial card draw
        // foreach(var entity in entities){
			
		// 	yield return StartCoroutine(entity.draw(4));
        // }
		yield return UCoroutine.YieldInParallel(
				entities.Select(entity => entity.draw(4))
			.ToArray());
		
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
}
