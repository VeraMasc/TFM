using System.Collections.Generic;
using UnityEngine;

namespace CardHouse
{
    public class CardGridLayout : CardGroupSettings
    {
        public int CardsPerRow = 5;
        [Tooltip("Vertical offset")]
        public float MarginalCardOffset = 0.05f;
        Collider2D MyCollider;
        public bool Straighten = true;

        void Awake()
        {
            MyCollider = GetComponent<Collider2D>();
            if (MyCollider == null)
            {
                Debug.LogWarningFormat("{0}:{1} needs Collider2D on its game object to function.", gameObject.name, "GridLayout");
            }
        }

        protected override void ApplySpacing(List<Card> cards, SeekerSetList seekerSets)
        {
            var width = transform.lossyScale.x;
            var height = transform.lossyScale.y;

            var rowCount = CardsPerRow >0? 1 + (cards.Count - 1) / CardsPerRow
                : 1;
            var realCardsPerRow = CardsPerRow >0? CardsPerRow : cards.Count;
            var colSpacing = height / (rowCount + 1);

            for (var row = 0; row < rowCount; row++)
            {
                var cardsInThisRow = Mathf.Min(realCardsPerRow, cards.Count - row * realCardsPerRow);
                var rowSpacing = width / (cardsInThisRow + 1);
                for (var col = 0; col < cardsInThisRow; col++)
                {
                    var newPos = transform.position
                                 + transform.right * width * -0.5f
                                 + transform.right * (col + 1) * rowSpacing
                                 + transform.up * height * 0.5f
                                 + transform.up * (row + 1) * colSpacing * -1
                                 + transform.forward * (MountedCardAltitude + MarginalCardOffset * (row * realCardsPerRow + col)) * -1;

                    var cardIndex = row * realCardsPerRow + col;
                    var card = cards[cardIndex];
                    var seekerSet = seekerSets?.GetSeekerSetFor(card);
                    card.Homing.StartSeeking(newPos, seekerSet?.Homing);
                    if (Straighten)
                    {
                        card.Turning.StartSeeking(transform.rotation.eulerAngles.z, seekerSet?.Turning);
                    }
                    card.Scaling.StartSeeking(UseMyScale ? groupScale : 1, seekerSet?.Scaling);
                }
            }
        }
    }
}