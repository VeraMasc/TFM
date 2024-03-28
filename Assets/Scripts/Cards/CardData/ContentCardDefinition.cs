using UnityEngine;
using CardHouse;
using CustomInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using System.IO;


/// <summary>
/// Define las propiedades que tiene cada carta de habitación
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Content")]
public class ContentCardDefinition : CardDefinition
{

    [TextArea(10, 100)]
    public string cardText;

    public Sprite Art;


    [SerializeField]
    public CardEffects effects;

}

