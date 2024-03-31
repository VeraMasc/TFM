using UnityEngine;
using CardHouse;
using CustomInspector;


/// <summary>
/// Define las propiedades que tiene cada carta de habitación
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Room")]
public class RoomCardDefinition : CardDefinition
{
    public string cardName;
    
    [TextArea(10, 100)]
    public string cardText;
    /// <summary>
    /// Cantidad de contenido con el que llenar la habitación
    /// </summary>
    [Min(0)]
    public int size;

    /// <summary>
    /// Cantidad de salidas que tiene cada habitación
    /// </summary>
    [Min(1)]
    public int exits;

    
    [Set]
    public SerializableSet<RoomTypes> roomTypes;
    
    public Sprite Art;

    private void Awake() {
        cardName = name;
    }
}

