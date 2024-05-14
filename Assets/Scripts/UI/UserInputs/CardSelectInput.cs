using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Linq;

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

    public Transform displayRoot;
    
    // Start is called before the first frame update
    void Start()
    {
        displayRoot.position = new Vector3(0,0, displayRoot.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Añade una lista de cartas como opciones
    /// </summary>
    public void addCardOptions(IEnumerable<Card> cards){

        var prefab = GameUI.singleton.prefabs.cardSelectOption;
        var index = 0;
        foreach(var card in cards){
            var instance = Instantiate(prefab, displayRoot);
            instance.parent = this;
            instance.index = index;
            instance.setSourceCard(card);
            index++;
        }
    }

    /// <summary>
    /// Añade una lista de cartas como opciones
    /// </summary>
    public void addModalOptions(Card basecard){

    }

    [NaughtyAttributes.Button]
    public void test(){
        var cards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Take(3).ToArray();

        addCardOptions(cards);
    }
}
