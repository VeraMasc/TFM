using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFlow;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Lista de todas las entidades
    /// </summary>
    public List<Entity> entities;
    


    private static GameController _singleton;
	///<summary>Controller Singleton</summary>
	public static GameController singleton
	{
		get 
		{
			if (_singleton == null)
			{
				_singleton = FindObjectOfType<GameController>(); //Para cuando el maldito hotreload me pierde la referencia
			}
			return _singleton;
		}
	}

    void Awake()
    {
        if( this != (_singleton ??=this )){
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ITargetable[] getAllTargetablesOfType<T>(){
        var type = typeof(T);
        if(typeof(T) == typeof(Entity)){
            return entities.ToArray();
        }
        //TODO: getAllTargetablesOfType<T>
        throw new NotImplementedException();
    }
    public ITargetable[] getAllOfType(Context context){
        //TODO: getAllTargetablesOfType
        if(context?.self?.GetType() == typeof(Entity)){
            return entities.ToArray();
        }
        throw new NotImplementedException();
    }

    /// <summary>
    /// Obtiene todos los miembros de un equipo
    /// </summary>
    public IEnumerable<Entity> getMembers(EntityTeam team){
        return entities.Where(e => e.team == team);
    }

    /// <summary>
    /// Obtiene todos los enemigos de un equipo
    /// </summary>
    public IEnumerable<Entity> getEnemies(EntityTeam team){
       
        if(team == EntityTeam.player){
            return entities.Where(e => e.team == EntityTeam.enemy);

        }else if(team == EntityTeam.enemy){
            return entities.Where(e => e.team == EntityTeam.player);

        }else{ //El resto de teams no tienen enemigos
            return Enumerable.Empty<Entity>();
        }
    }


}
