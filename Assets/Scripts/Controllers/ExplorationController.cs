using System.Collections;

using UnityEngine;
using CardHouse;
using System.Linq;
using GameFlow;
using Ra.Trail;
using CustomInspector;
using Common.Coroutines;


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
    /// Stack de resolución de efectos
    /// </summary>
    
    public CardResolveOperator effectStack;

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
        var room = currentRoom.MountedCards.FirstOrDefault()?.GetComponent<RoomCard>();
        var amount = room?.exits ?? optionAmount;
        var transfer = roomOptions.GetComponent<CardTransferOperator>();
        transfer.NumberToTransfer = amount;
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
        
        var cards = content.Get(GroupTargetType.Last,2);

        foreach(var room in roomOptions.MountedCards){
            var roomData = room.GetComponent<RoomCard>();
            if(room.attachedGroup == null){
                room.attachedGroup =  Instantiate<CardGroup>(attachPrefab, room.transform);
            }
            //Get room data
            var contentSize = roomData?.size ?? roomContentDefault;
            transfer.Transition.Destination = room.attachedGroup;
            transfer.NumberToTransfer = contentSize;
            transfer.Activate();
            
        }

        yield return new WaitForEndOfFrame();
        yield return transfer.currentAction;

        //Flip cards up
        foreach(var room in roomOptions.MountedCards){
            var firstContent = room.attachedGroup.Get(0);
            yield return StartCoroutine(revealContent(firstContent));
        }
    }


    /// <summary>
    /// Se ejecuta al escoger la siguiente habitación e inicia todos los efectos asociados
    /// </summary>
    public void onRoomChosen(){
        if(currentRoom.MountedCards.Count >0)
            StartCoroutine(onRoomChosenCoroutine());
    }

    private IEnumerator onRoomChosenCoroutine(){
        clearUnchosenOptions();
        
        var room = currentRoom.MountedCards.First();
        var hiddenContent = room.attachedGroup.MountedCards
            .Where((Card c) => c.Facing != CardFacing.FaceUp);


       
        yield return new WaitForSeconds(1);

        foreach(var content in hiddenContent.ToArray()){
            var minTime = new WaitForSeconds(0.5f);
            yield return UCoroutine.Yield(revealContent(content));
            yield return minTime;//Wait extra if flip is too fast
        }
    }

    

    /// <summary>
    /// Añade contenido a la habitación escogida
    /// </summary>
    /// <param name="amount"></param>
    public void addContent(Card room ,int amount){
        var transfer = content.GetComponent<CardTransferOperator>();
        transfer.Transition.Destination = room.attachedGroup;
        transfer.NumberToTransfer = amount;
        transfer.Activate();
    }

    /// <summary>
    /// Revela el contenido
    /// </summary>
    public IEnumerator revealContent(Card contentCard){
        
        contentCard.SetFacing(CardFacing.FaceUp);
        yield return UCoroutine.YieldAwait( ()=> !contentCard.FaceTurning.seeking);
    
        var data = (ContentCard) contentCard.data;

        if(data?.effects?.revealEffect?.isEmpty==false){
            yield return CardTransferOperator.sendCard(contentCard,effectStack.stack);
            yield return effectStack.waitTillEmpty;
        }
        
        
    }
}
