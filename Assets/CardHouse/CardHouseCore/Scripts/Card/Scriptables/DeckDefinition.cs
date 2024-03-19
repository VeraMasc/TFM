using System;
using System.Collections.Generic;
using CustomInspector;
using NaughtyAttributes;
using UnityEngine;

namespace CardHouse
{
    [CreateAssetMenu(menuName = "CardHouse/Deck Definition")]
    public class DeckDefinition : ScriptableObject, IDeckDefinition
    {
        [SerializeField]
        private Sprite cardBackArt;

        public Sprite CardBackArt { get => cardBackArt; set => cardBackArt = value; }
        [SerializeField]
        private List<CardDefinition> cardCollection;

        
        public List<CardDefinition> CardCollection { get => cardCollection; set => cardCollection = value; }
    }

    public interface IDeckDefinition{
        public Sprite CardBackArt { get; set; }
        public List<CardDefinition> CardCollection { get;}
    }
}
