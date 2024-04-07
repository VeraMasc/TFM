using System;
using System.Collections.Generic;
using CustomInspector;
using NaughtyAttributes;
using UnityEngine;
using CardHouse;
using System.Linq;


/// <summary>
/// Describe el estado actual de una decklist concreta
/// </summary>
[Serializable]
public class DecklistState 
{
    [Unfold]
    public List<CardCopies> inDeck;
    [Unfold]
    public List<CardCopies> spent;
    [Unfold]
    public List<CardCopies> exiled;
}


