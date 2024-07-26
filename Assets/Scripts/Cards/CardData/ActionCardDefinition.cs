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
    /// <summary>
    /// Velocidad de uso de la carta
    /// </summary>
    public SpeedTypes speedType;
    [SerializeField]
    List<ActionSubtypes> typeList;

    [SerializeField]
    CardTypeEnum typeListTest;

    


    /// <summary>
    /// Coste de la carta
    /// </summary>
    [Unfold]
    public ManaCost cost;

    [SerializeReference, SubclassSelector]
    public CardHeuristic heuristic = new SimpleHeuristic();

    protected override void Awake() {
        base.Awake();
        // effects = (ContentCardEffects)effects.cloneAll();
    }
    public override void OnValidate() {
        base.OnValidate();
        heuristic?.validate();
    }

    public override string getCardTypes()
    {
        return string.Join(", ",typeList);
    }
    
}

public enum SpeedTypes{
    Action,
    Reaction,
    Ritual
}

public enum ActionSubtypes{
    Spell,
    Melee,
    Hex,
    Blessing,
    Enchantment,
    Mental,
    Aura,
    Ranged
}