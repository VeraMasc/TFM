using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

/// <summary>
/// Gestiona los selectores de cartas o de opciones modales
/// </summary>
public class CardSelectInput : PlayerInputBase
{
    /// <summary>
    /// Cantidad máxima de opciones que se pueden escoger simultáneamente
    /// </summary>
    public int maxChoices = 1;

    /// <summary>
    /// Lista con los índices de las opciones escogidas
    /// </summary>
    public List<int> chosen;

    /// <summary>
    /// Lista de opciones entre las que escoger
    /// </summary>
    public List<CardSelectOption> options;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Añade una lista de cartas como opciones
    /// </summary>
    public void addCardOptions(IEnumerable<Card> cards){

    }

    /// <summary>
    /// Añade una lista de cartas como opciones
    /// </summary>
    public void addModalOptions(Card basecard){

    }
}
