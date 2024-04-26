using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// Singleton que gestiona la interfaz
/// </summary>
public class GameUI : MonoBehaviour
{
    [HorizontalLine(3,message = "General")]
    public Canvas canvas;

    /// <summary>
    /// Posición en la que mostrar los detalles de la carta seleccionada
    /// </summary>
    public Transform cardDetails;

    /// <summary>
    /// Posición en la que mostrar la mano del personaje seleccionado
    /// </summary>
    public Transform handDetails;

    /// <summary>
    /// Layout para el grupo de cartas expandido
    /// </summary>
    [HorizontalLine(3,message = "Group spread")]
    public SpreadLayout spread;

    /// <summary>
    /// Scrollbar del grupo de cartas expandido
    /// </summary>
    public Slider spreadScrollbar;

    /// <summary>
    /// Grupo de cartas expandido actualmente
    /// </summary>
    public CardGroup focusGroup;

    /// <summary>
    /// Propiedad para facilitiar el acceso al focus group
    /// </summary>
    public static CardGroup focus => singleton?.focusGroup;

    /// <summary>
    /// Listado de los prefabs de la interfaz
    /// </summary>
    [HorizontalLine(3,message = "User Inputs")]
    public UIPrefabs prefabs;

    /// <summary>
    /// Objeto donde se genera la UI temporal que se usa para gestionar los distintos inputs
    /// </summary>
    public Transform userInputRoot;

    /// <summary>
    /// User input que hay activo en este momento
    /// </summary>
    public PlayerInputBase activeUserInput;

    /// <summary>
    /// Lista de targets válidos
    /// </summary>
    public IEnumerable<ITargetable> possibleTargets;

    /// <summary>
    /// Indica si la interfaz está ocupada esperando algún input
    /// </summary>
    public bool isBusy{
        get => activeUserInput != null;
    }

    /// <summary>
    /// Cambia el focus group
    /// </summary>
    /// <param name="group"></param>
    public static void setFocus(CardGroup group){
        var old = singleton?.focusGroup;
        if(singleton){ //Evita errores si no hay UI
            if(old != group){
                singleton.focusGroup = group;
                singleton.spreadScrollbar?.gameObject.SetActive(true);
                resetFocusScroll();
                
            }else{
                singleton.focusGroup = null; //Deshacer focus
                singleton.spreadScrollbar?.gameObject.SetActive(false);
            }
            
        }
        old?.ApplyStrategy(); //Devolver las cartas a su estrategia anterior
    }

    /// <summary>
    /// Actualiza la posición de las cartas con focus
    /// </summary>
    public static void refreshFocus(){
        if(singleton) {
            singleton.focusGroup?.ApplyStrategy();
        }
    }

    /// <summary>
    /// Resetea el scroll
    /// </summary>
    public static void resetFocusScroll(){
        if(singleton) {
            //singleton.spreadScrollbar.value =0;
            singleton.spreadScrollbar.normalizedValue=0;
            singleton.spreadScrollbar.onValueChanged.Invoke(0);
        }
    }


    /// <summary>
    /// Genera la interfaz del input y espera a que devuelva un valor
    /// </summary>
    /// <returns></returns>
    public IEnumerator getInput(PlayerInputBase input, Action<object> returnAction){
        clearInputs();
        //Crea un bloqueador para inpedir que los inputs traspasen
        Instantiate(prefabs.clickBlocker,userInputRoot);
        
        //Crea la interfaz de input (HA DE IR DESPUÉS DEL BLOQUEADOR)
        var instance = Instantiate(input,userInputRoot);
        activeUserInput = instance;
        yield return activeUserInput.waitTillFinished;
        if(!activeUserInput.isCancelled){
            returnAction.Invoke(instance.inputValue);
        }
        clearInputs();
    }

    /// <summary>
    /// Genera la interfaz de confirmación y espera a que se escojan los targets
    /// </summary>
    public IEnumerator getTargets(IEnumerable<ITargetable> targetables, Action<object> returnAction){
        clearInputs();
        possibleTargets = targetables;

        //TODO: add checks for invalid inputs
        
        //Crea la interfaz de confirmación
        var instance = Instantiate(prefabs.confirmationInput, userInputRoot);
        activeUserInput = instance;
        yield return activeUserInput.waitTillFinished;
        if(!activeUserInput.isCancelled){
            throw new NotImplementedException("Falta seleccionar targets");
        }
        clearInputs();
    }

    /// <summary>
    /// Elimina todos los user inputs
    /// </summary>
    public void clearInputs(){
        foreach(Transform  child in userInputRoot){
            Destroy(child.gameObject);
        }
    }

    private static GameUI _singleton;
	///<summary>UI Singleton</summary>
	public static GameUI singleton
	{
		get 
		{
			if (_singleton == null)
			{
				_singleton = FindObjectOfType<GameUI>(); //Para cuando el maldito hotreload me pierde la referencia
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
            closeCardDetails();
            hit.collider.SendMessage("onRightClick",SendMessageOptions.DontRequireReceiver);
        }
        else{closeCardDetails();}
    }

    /// <summary>
    /// Cierra el popup de detalles de la carta cuando ya no se necesita
    /// </summary>
    public void closeCardDetails(){
        foreach (Transform  child in cardDetails){
            Destroy(child.gameObject);
        }
        
    }
}
