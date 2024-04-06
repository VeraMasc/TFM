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
    /// Renderer de la cara frontal de la carta
    /// </summary>
    public SpriteRenderer Image;

    /// <summary>
    /// Renderer de la cara trasera de la carta
    /// </summary>
    public SpriteRenderer BackImage;

    /// <summary>
    /// Base textbox for the card
    /// </summary>
    public TextMeshPro cardTextBox;

    /// <summary>
    /// Tipos de la carta en cuestión
    /// </summary>
    public virtual HashSet<string> cardType {get;set;}
}