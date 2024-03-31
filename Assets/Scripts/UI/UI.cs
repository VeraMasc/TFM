using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Singleton que gestiona la interfaz
/// </summary>
public class UI : MonoBehaviour
{
    public Canvas canvas;

    public GameObject cardDetails;

    private static UI _singleton;
	///<summary>UI Singleton</summary>
	public static UI singleton
	{
		get 
		{
			if (_singleton == null)
			{
				_singleton = FindObjectOfType<UI>(); //Para cuando el maldito hotreload me pierde la referencia
			}
			return _singleton;
		}
	}

    void Awake()
    {
        _singleton =this;
    }


}
