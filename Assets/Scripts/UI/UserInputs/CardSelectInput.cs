using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Linq;
using Effect;

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

    public override void setInputConfig(InputParameters parameters){
        //Generate cards
        if(parameters?.values is ModalOptionSettings[] modals){
            addModalOptions((Card)parameters.context.self, modals);
        }else if (parameters?.values is Card[] cards){
            addCardOptions(cards);
        }

        //Set other config
        if(parameters.extraConfig is ExtraInputOptions extra){
            maxChoices = extra.maxChoices;
        }
    }

    public override void confirmChoice(){
        inputValue = chosen;
        isFinished=true;
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
    public void addModalOptions(Card basecard, IEnumerable<ModalOptionSettings> modes){
        var prefab = GameUI.singleton.prefabs.cardSelectOption;
        var index = 0;
        foreach(var mode in modes){
            var instance = Instantiate(prefab, displayRoot);
            instance.parent = this;
            instance.index = index;
            instance.setSourceCard(basecard);
            instance.setMode(mode);
            index++;
        }
    }

    [NaughtyAttributes.Button]
    public void test(){
        var cards = FindObjectsByType<Card>(FindObjectsSortMode.None)
            .Take(3).ToArray();

        addCardOptions(cards);
    }
    public class ExtraInputOptions{
        public int maxChoices;
    }
}
/// <summary>
/// Permite configurar las opciones modales
/// </summary>
public class ModalOptionSettings{
    public string tag;
    public string text;
    public Ability ability;

    /// <summary>
    /// Desactiva la opción en el selector
    /// </summary>
    public bool disabled;
}