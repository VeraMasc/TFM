using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Linq;
using Effect;
using System;

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

    /// <summary>
    /// Raiz del display
    /// </summary>
    public Transform displayRoot;

    /// <summary>
    /// Objeto que contiene la lista de opciones
    /// </summary>
    public Transform displayList;

    public SpriteRenderer background;

    public SliderScrollbar scrollbar;

    public float scaleDown = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        updateResolution();
    }

    void OnPreRender()
    {
        updateResolution();
        
    }

    

    void LateUpdate()
    {
        
        updateResolution();
        enableScrollIfNeeded();
    }

    public void updateResolution(){
        displayRoot.position = new Vector3(0,0, displayRoot.position.z);
        displayRoot.localScale = 1080f/Screen.height/scaleDown * new Vector3(1,1,0)
            + Vector3.forward;

        background.transform.position= displayRoot.position + Vector3.forward;
    }

    public override void setInputConfig(InputParameters parameters){
        //Generate cards
        if(parameters?.values is ModalOptionSettings[] modals){
            addModalOptions((Card)parameters.context.self, modals);
        }else if (parameters?.values is Card[] cards){
            addCardOptions(cards);
        }
        else if (parameters?.values is Mana[][] manaOptions){
            var asText = manaOptions.Select(
                arr =>{ 
                    
                    var raw = string.Join("",arr.Select(m => $"{{{m}}}"));
                    return MyCardDefinition.parseCost(raw);
                });
                
            addTextOptions(asText);
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
            var instance = Instantiate(prefab, displayList);
            instance.parent = this;
            instance.index = index;
            instance.setSourceCard(card);
            index++;
        }
        enableScrollIfNeeded();
    }


    public void addTextOptions(IEnumerable<string> strings){

        var prefab = GameUI.singleton.prefabs.textSelectOption;
        var index = 0;
        foreach(var str in strings){
            var instance = Instantiate(prefab, displayList);
            instance.parent = this;
            instance.index = index;
            instance.setText(str);
            index++;
        }
        enableScrollIfNeeded();
    }

    /// <summary>
    /// Añade una lista de cartas como opciones
    /// </summary>
    public void addModalOptions(Card basecard, IEnumerable<ModalOptionSettings> modes){
        var prefab = GameUI.singleton.prefabs.cardSelectOption;
        var index = 0;
        foreach(var mode in modes){
            var instance = Instantiate(prefab, displayList);
            instance.parent = this;
            instance.index = index;
            instance.setSourceCard(basecard);
            instance.setMode(mode);
            index++;
        }
        enableScrollIfNeeded();
    }

    public void enableScrollIfNeeded(){
        if(scrollbar.gameObject.activeInHierarchy)
            return;
        
        var listRect = displayList as RectTransform;
        var rootRect = displayRoot as RectTransform;
        if(listRect.sizeDelta.x > rootRect.sizeDelta.x){
            scrollbar.gameObject.SetActive(true);
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
    /// Indica que hay que reemplazar el coste con toro
    /// </summary>
    public ManaCost replaceCost;


    /// <summary>
    /// Desactiva la opción en el selector
    /// </summary>
    public bool disabled;
}