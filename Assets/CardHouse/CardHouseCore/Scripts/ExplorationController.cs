using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;

/// <summary>
/// Gestiona todo lo relacionado con la exploración
/// </summary>
public class ExplorationController : MonoBehaviour
{
    public CardGroup rooms;

    public CardGroup currentRoom;

    public int optionAmount =2;
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

    public void clearUnchosenOptions(){
        var transfer = discarded.GetComponent<CardTransferOperator>();
        transfer.Activate();

    }

}
