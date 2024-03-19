using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;
using Unity.VisualScripting;
using System.Threading;


/// <summary>
/// Gestiona todo lo relacionado con la exploración <see cref="ExplorationController.content" />
/// </summary>
public class ExplorationController : MonoBehaviour
{
    public CardGroup rooms;

    public CardGroup currentRoom;

    public int optionAmount =2;

    //TODO: remove after testing
    public int roomContentDefault =2;
    public CardGroup roomOptions;

    public CardGroup content;

    /// <summary>
    /// Contenido de la habitación actual
    /// </summary>
    public CardGroup currentContent;

    /// <summary>
    /// Pila de cartas descartadas
    /// </summary>
    public CardGroup discarded;

    /// <summary>
    /// Prefab of the room attach group
    /// TODO: que sea globalmente accesible
    /// </summary>
    [CustomInspector.AssetsOnly]
    public CardGroup attachPrefab;

    private static ExplorationController _singleton;
	///<summary>Controller Singleton</summary>
	public static ExplorationController singleton
	{
		get 
		{
			if (_singleton == null)
			{
				_singleton = FindObjectOfType<ExplorationController>(); //Para cuando el maldito hotreload me pierde la referencia
			}
			return _singleton;
		}
	}

    void Awake()
    {
        _singleton =this;
    }

    public void Start()
    {
        getNextOptions();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U)){

        }
    }

    /// <summary>
    /// Pone las siguientes opciones sobre la mesa
    /// </summary>
    public void getNextOptions(){
        var transfer = roomOptions.GetComponent<CardTransferOperator>();
        transfer.NumberToTransfer = optionAmount;
        transfer.Activate();
    }

    /// <summary>
    /// Limpia la zona de selección de opciones
    /// </summary>
    public void clearUnchosenOptions(){
        foreach (var room in roomOptions.MountedCards){
            room.attachedGroup?.destroyGroup(GroupName.Discard);
        }
        var transfer = discarded.GetComponent<CardTransferOperator>();
        transfer.Activate();

    }

    /// <summary>
    /// Asocia el contenido a cada habitación
    /// </summary>
    public void attachContent(){
        StartCoroutine(attachContentCorroutine());
    }

    private IEnumerator attachContentCorroutine(){
        var transfer = content.GetComponent<CardTransferOperator>();
        var contentSize = roomContentDefault;
        var cards = content.Get(GroupTargetType.Last,2);

        foreach(var room in roomOptions.MountedCards){
            if(room.attachedGroup == null){
                room.attachedGroup =  Instantiate<CardGroup>(attachPrefab, room.transform);
            }
            //TODO: Make static transfer operators that can be used without monobehaviours
            transfer.Transition.Destination = room.attachedGroup;
            transfer.NumberToTransfer = contentSize;
            transfer.Activate();
            
        }

        yield return new WaitForEndOfFrame();
        yield return transfer.currentAction;

        //Flip cards up
        foreach(var room in roomOptions.MountedCards){
            var firstContent = room.attachedGroup.Get(0);
            firstContent.SetFacing(CardFacing.FaceUp);
        }
    }


}
