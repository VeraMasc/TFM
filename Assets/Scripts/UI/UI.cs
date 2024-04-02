using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;



/// <summary>
/// Singleton que gestiona la interfaz
/// </summary>
public class UI : MonoBehaviour
{
    public Canvas canvas;

    public SpriteRenderer cardDetails;

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

    void Update()
    {
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("rightClick");
            rightClickRaycast();
        }
    }

    /// <summary>
    /// Activa el evento "onRightClick" en la posición actual del mouse
    /// </summary>
    void rightClickRaycast(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null) {
            hit.collider.SendMessage("onRightClick",SendMessageOptions.DontRequireReceiver);

            if(hit.collider.GetComponent<Card>() == null){
                closeCardDetails();
            }
        }
        else{closeCardDetails();}
    }

    /// <summary>
    /// Cierra el popup de detalles de la carta cuando ya no se necesita
    /// </summary>
    public void closeCardDetails(){
        cardDetails.enabled=false;
    }
}
