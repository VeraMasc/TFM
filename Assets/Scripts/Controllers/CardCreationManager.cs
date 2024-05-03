using System;
using CardHouse;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Se encarga de gestionar la creación de cartas a partir de sus definiciones
/// </summary>
[CreateAssetMenu(menuName = "TFM/CardCreationManager")]
public class CardCreationManager : ScriptableObject
{
    [HorizontalLine(3, message ="Card Setups")]
    /// <summary>
    /// Prefab de cartas de acción
    /// </summary>
    [AssetsOnly]
    public ActionCard actionPrefab;

    [AssetsOnly]
    public RoomCard roomPrefab;

    [AssetsOnly]
    public ContentCard contentPrefab;

    [AssetsOnly]
    public TriggerCard triggerPrefab;

    [HorizontalLine(3, message ="Other Prefabs")]

    [AssetsOnly]
    public CardGroup attachGroup;


    /// <summary>
    /// Crea una instancia de una carta a partir de su definición sin necesidad de un prefab
    /// </summary>
    /// <param name="definition">definición de la carta</param>
    /// <param name="position">posición en la que crearla</param>
    /// <returns>la carta creada</returns>
    public Card create(CardDefinition definition, Vector3 position = default(Vector3)){
        var prefab = getSetup(definition);

        if(prefab == null){
            Debug.LogError($"Can't find card setup for {definition}");
            return null;
        }
        
        var setup = Instantiate(prefab, position, Quaternion.identity);
        setup.Apply(definition);
        return setup.GetComponent<Card>();
    }

    /// <summary>
    /// Creates a copy of a card
    /// </summary>
    /// <param name="card"></param>
    /// <param name="inPlace">indica si hay que generar la nueva carta en el mismo sitio que la original</param>
    /// <returns></returns>
    public Card duplicate(Card card, bool inPlace = true){
        var data = card.data ?? card.GetComponent<CardSetup>();
        if(data is MyCardSetup setup){
            var position = inPlace? card.transform.position: Vector3.zero;
            var newCard = create(setup.definition, position);
            if(inPlace){
                var index = card.Group.MountedCards.FindIndex(c=> c == card);
                card.Group.Mount(newCard,index+1);
            }
            return newCard; 
        }
        return null;
    }

    /// <summary>
    /// Obtiene el prefab que usa la carta en base a su definición
    /// </summary>
    public MyCardSetup getSetup(CardDefinition definition){
        switch (definition){

            case ActionCardDefinition:
                return actionPrefab;

            case ContentCardDefinition:
                return contentPrefab;

            case RoomCardDefinition:
                return roomPrefab;

            default:
                return null;
        }
    }

}