using UnityEngine;
using CardHouse;
using CustomInspector;
using UnityEditor;
using System.IO;
using Effect;
using System.Collections.Generic;


/// <summary>
/// Define las propiedades que tiene cada carta de acción
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Action")]
public class ActionCardDefinition : MyCardDefinition
{

    [SerializeField]
    List<ActionSubtypes> typeList;


    /// <summary>
    /// Coste de la carta
    /// </summary>
    [Unfold]
    public ManaCost cost;

    [SerializeField,BackgroundColor(FixedColor.Purple)]
    public BaseCardEffects effects;

    protected override void Awake() {
        base.Awake();
        // effects = (ContentCardEffects)effects.cloneAll();
    }

    public override string getCardTypes()
    {
        return string.Join(", ",typeList);
    }
}

public enum ActionSubtypes{
    Spell,
    Melee,
    Hex,
    Blessing,
    Enchantment,
    Mental
}