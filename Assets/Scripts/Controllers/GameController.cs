using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Lista de todas las entidades
    /// </summary>
    public List<EntityController> entities;


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
}
