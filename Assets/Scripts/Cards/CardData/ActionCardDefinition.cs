using UnityEngine;
using CardHouse;
using CustomInspector;

using UnityEditor;
using UnityEditor.PackageManager;
using System.IO;


/// <summary>
/// Define las propiedades que tiene cada carta de acción
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Action")]
public class ActionCardDefinition : MyCardDefinition
{


    [SerializeField]
    public BaseCardEffects effects;

    
}

