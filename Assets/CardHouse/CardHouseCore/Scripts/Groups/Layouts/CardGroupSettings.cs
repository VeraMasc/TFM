using System.Collections.Generic;
using UnityEngine;

namespace CardHouse
{
    public abstract class CardGroupSettings : MonoBehaviour
    {
        public int CardLimit = -1; // Limit < 0 means no limit
        public float MountedCardAltitude = 0.01f;
        public CardFacing ForcedFacing;
        /// <summary>
        /// Forces interactability rules within group
        /// </summary>
        public GroupInteractability ForcedInteractability;

        /// <summary>
        /// Reduce el renderizado a solo la carta superior a no ser que se le haga focus
        /// </summary>
        public bool compactDisplay;
        public MountingMode DragMountingMode = MountingMode.Top;
        public bool UseMyScale = false;

        /// <summary>
        /// Aplica la configuración del grupo a una lista de cartas
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="instaFlip"></param>
        /// <param name="seekerSets"></param>
        public void Apply(List<Card> cards, bool instaFlip = false, SeekerSetList seekerSets = null)
        {
            for (var i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                if (ForcedFacing != CardFacing.None)
                {
                    var flipSpeed = 1f;
                    if (seekerSets != null && seekerSets.Count > 0 && seekerSets[0] != null)
                    {
                        flipSpeed = seekerSets[0].FlipSpeed;
                    }
                    cards[i].SetFacing(ForcedFacing, immediate: instaFlip, spd: flipSpeed);
                }
                if (ForcedInteractability != GroupInteractability.None)
                {
                    var col = card.GetComponent<Collider2D>();
                    if (col)
                    {
                        col.enabled = ForcedInteractability == GroupInteractability.Active
                                      || ForcedInteractability == GroupInteractability.OnlyTopActive && i == cards.Count - 1;
                    }
                    //Desactivar el renderizado de todas las cartas menos la primera
                    card.transform.GetChild(0).gameObject.SetActive(!compactDisplay ||(i == cards.Count - 1));
                    
                }
            }

            ApplySpacing(cards, seekerSets);
        }

        protected abstract void ApplySpacing(List<Card> cards, SeekerSetList seekerSets);
    }
}
