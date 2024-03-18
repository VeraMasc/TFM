using UnityEngine;
using CardHouse;


/// <summary>
/// Se encarga de la generación de las cartas de habitación
/// </summary>
public class RoomCard : CardSetup
{
    public SpriteRenderer Image;
    public SpriteRenderer BackImage;
    
    public string cardText { get; private set; }
    public int size { get; private set; }

    public int exits { get; private set; }

    /// <summary>
    /// Aplica la configuración de la carta
    /// </summary>
    public override void Apply(CardDefinition data)
    {
        if (data is PokerCardDefinition pokerCard)
        {
            Image.sprite = pokerCard.Art;
            if (pokerCard.BackArt != null)
            {
                BackImage.sprite = pokerCard.BackArt;
            }
        }
    }
}

