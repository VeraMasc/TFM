using System;
using System.Collections.Generic;
using CustomInspector;
using NaughtyAttributes;
using UnityEngine;
using CardHouse;
using System.Linq;
using UnityEngine.Serialization;


/// <summary>
/// Definición mejorada de mazo que permite controlar más fácilmente el número de copias que <see cref="CardHouse.DeckDefinition"/>
/// </summary>
[CreateAssetMenu(menuName = "Cards/DecklistDefinition")]
public class DecklistDefinition : ScriptableObject, IDeckDefinition
{
    [SerializeField]
    private Sprite cardBackArt;
        
    public Sprite CardBackArt { get => cardBackArt; set => cardBackArt = value; }
    
    public List<CardDefinition> CardCollection {
        get { 
           return CardsetCollection.SelectMany( set => { 
                return Enumerable.Repeat(set.card as CardDefinition, Math.Max(0,set.amount));
           }).ToList();
        }
    }

    /// <summary>
    /// Tamaño total del mazo
    /// </summary>
    /// <returns></returns>
    [CustomInspector.ReadOnly]
    public int deckSize;

    
    
    [Unfold]
    public List<CardCopies> CardsetCollection;

    public List<LearnedSkills> skills;

    private void OnValidate() {
        deckSize = CardsetCollection?.Select(c => c.amount).Sum() ??0;
    }
}


[Serializable]
public struct CardCopies{
    [HorizontalGroup(true,size = 2)]
    [FormerlySerializedAs("card")]
    public MyCardDefinition card;
    [HorizontalGroup(size = 1)]
    public int amount;
}

[Serializable]
public struct LearnedSkills{
    [HorizontalGroup(true,size = 2)]
    public SkillCardDefinition card;
    [HorizontalGroup(size = 1)]
    public int level;
}