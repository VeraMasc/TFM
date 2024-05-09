using UnityEngine;
using CardHouse;
using TMPro;
using System.Collections.Generic;
using CustomInspector;
using Button = NaughtyAttributes.ButtonAttribute;


/// <summary>
/// Es la base de todas las cartas de mi juego
/// </summary>
public abstract class MyCardSetup : CardSetup
{
    /// <summary>
    /// Scriptable object usado para generar esta carta
    /// </summary>
    [ReadOnly]
    public MyCardDefinition definition;

    /// <summary>
    /// Nombre de la carta
    /// </summary>
    public string cardName;

    /// <summary>
    /// Texto de la carta
    /// </summary>
    public string cardText;

    /// <summary>
    /// Tipo de la carta
    /// </summary>
    public string cardType;

    /// <summary>
    /// Renderer de la cara frontal de la carta
    /// </summary>
    public SpriteRenderer Image;

    /// <summary>
    /// Renderer del frame de la carta
    /// </summary>
    public SpriteRenderer FrameImage;

    /// <summary>
    /// Renderer de la cara trasera de la carta
    /// </summary>
    public SpriteRenderer BackImage;


    /// <summary>
    /// Textbox with the card name
    /// </summary>
    public TextMeshPro cardNameBox;

    /// <summary>
    /// Textbox with the card type
    /// </summary>
    public TextMeshPro cardTypeLine;

    /// <summary>
    /// Base textbox for the card
    /// </summary>
    public TextMeshPro cardTextBox;

    public BaseCardEffects effects;

    public T getEffectsAs<T>()where T:BaseCardEffects{
        return effects as T;
    }

    /// <summary>
    /// Tipos de la carta en cuestión
    /// </summary>
    public virtual HashSet<string> cardTypeSet {get;set;}

    /// <summary>
    /// Asigna los valores básicos de la carta
    /// </summary>
    /// <param name="data"></param>
    public override void Apply(CardDefinition data)
    {
        if(data is MyCardDefinition baseCard){
            this.cardName = baseCard.cardName;
            gameObject.name = baseCard.cardName;

            if(baseCard.Art != null){
                Image.sprite = baseCard.Art;
            }
            
            if (baseCard.BackArt != null)
            {
                BackImage.sprite = baseCard.BackArt;
            }
            cardText = baseCard.cardText;
            cardTextBox.text = cardText;
            cardName = baseCard.cardName;
            cardNameBox.text = cardName;

            cardType = baseCard.getCardTypes();
            if(cardTypeLine){
                cardTypeLine.text = cardType;
            }

            if(!GetType().IsSubclassOf(typeof(TriggerCard))){
                // poner definición solo si no es un trigger
                definition = baseCard;
            }
        }else{
            throw new System.Exception($"Can't initialize card with {data?.GetType()?.Name}");
        }
        
    }
}