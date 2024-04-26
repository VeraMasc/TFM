using UnityEngine;
using CardHouse;
using CustomInspector;

using UnityEditor;
using System.IO;
using Effect;


/// <summary>
/// Define las propiedades que tiene cada carta de acción
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Action")]
public class ActionCardDefinition : MyCardDefinition
{

    /// <summary>
    /// Coste de la carta
    /// </summary>
    [Unfold]
    public ManaCost cost;

    [SerializeField,BackgroundColor(FixedColor.Purple)]
    public BaseCardEffects effects;

    
}

