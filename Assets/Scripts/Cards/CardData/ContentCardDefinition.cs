using UnityEngine;
using CardHouse;
using CustomInspector;

using UnityEditor;
using UnityEditor.PackageManager;
using System.IO;


/// <summary>
/// Define las propiedades que tiene cada carta de habitación
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Content")]
public class ContentCardDefinition : MyCardDefinition
{


    [SerializeField]
    public ContentCardEffects effects;

    
}

