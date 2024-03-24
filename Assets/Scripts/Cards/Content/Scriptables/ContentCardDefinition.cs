using UnityEngine;
using CardHouse;
using CustomInspector;


/// <summary>
/// Define las propiedades que tiene cada carta de habitación
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Content")]
public class ContentCardDefinition : CardDefinition
{

    [TextArea(10, 100)]
    public string cardText;

    public Sprite Art;
}

