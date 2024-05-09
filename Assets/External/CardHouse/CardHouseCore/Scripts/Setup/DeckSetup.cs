using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace CardHouse
{
    public class DeckSetup : MonoBehaviour
    {
        public bool RunOnStart = true;

        /// <summary>
        /// Salta la animación de shuffle al inicializar el mazo
        /// </summary>
        public bool instaShuffle = true;
        public GameObject CardPrefab;
        [RequireType(typeof(IDeckDefinition)), SerializeReference]
        public ScriptableObject DeckDefinition;
        public CardGroup Deck;
        public List<TimedEvent> OnSetupCompleteEventChain;

        void Awake()
        {
            Deck ??= GetComponent<CardGroup>();
            Assert.IsNotNull(Deck);
        }
        void Start()
        {
            if (RunOnStart)
            {
                DoSetup();
            }
        }

        public void DoSetup()
        {
            StartCoroutine(SetupCoroutine());
        }

        IEnumerator SetupCoroutine()
        {
            var definition = DeckDefinition as IDeckDefinition;
            var newCardList = new List<Card>();
            foreach (var cardDef in definition.CardCollection)
            {
                var newThing = Instantiate(CardPrefab, Deck.transform.position, Deck.transform.rotation);
                newCardList.Add(newThing.GetComponent<Card>());
                var card = newThing.GetComponent<CardSetup>();

                if (card != null)
                {
                    var copyCardDef = cardDef;

                    if (cardDef.BackArt == null && definition.CardBackArt != null)
                    {
                        copyCardDef = Instantiate(cardDef);
                        copyCardDef.BackArt = definition.CardBackArt;
                    }
                    card.Apply(copyCardDef);
                }
            }

            yield return new WaitForEndOfFrame();

            foreach (var card in newCardList)
            {
                Deck.Mount(card, instaFlip: true);
            }

            Deck.Shuffle(instaShuffle);

            StartCoroutine(TimedEvent.ExecuteChain(OnSetupCompleteEventChain));
        }

        public static IEnumerator setupDeck(IDeckDefinition deck, CardGroup group, Entity owner, bool shuffle= true, CardFacing forceFacing = CardFacing.FaceDown){
            var newCardList = new List<Card>();
            foreach (var cardDef in deck.CardCollection)
            {
                var prefab = GameController.singleton.creationManager.getSetup(cardDef);
                var newThing = Instantiate(prefab, group.transform.position, group.transform.rotation);
                newCardList.Add(newThing.GetComponent<Card>());
                var card = newThing.GetComponent<CardSetup>();

                if(owner!=null){
                    var ownership = newThing.gameObject.GetComponent<CardOwnership>();
                    ownership.setOwner(owner);
                }

                if (card != null)
                {
                    var copyCardDef = cardDef;

                    if (cardDef.BackArt == null && deck.CardBackArt != null)
                    {
                        copyCardDef = Instantiate(cardDef);
                        copyCardDef.BackArt = deck.CardBackArt;
                    }
                    card.Apply(copyCardDef);

                    
                }

                
            }
            yield return new WaitForEndOfFrame();

            foreach (var card in newCardList)
            {
                card.SetFacing(forceFacing,true);
                group.Mount(card, instaFlip: true);
            }

            if(shuffle)
                group.Shuffle(true);
        }
    }
}
