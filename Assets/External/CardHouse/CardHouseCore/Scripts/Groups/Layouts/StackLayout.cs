using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CardHouse
{
    public class StackLayout : CardGroupSettings
    {
        public Vector3 MarginalCardOffset = new Vector3(0.01f, 0.01f, -0.01f);
        public TriggerEnterRelay SecondaryCollider;
        public bool Straighten = true;

        

        
        protected override void ApplySpacing(List<Card> cards, SeekerSetList seekerSets = null)
        {
            
            if(group is CompactCardGroup compact){ //Cambia el display si hay focus
                if(GameUI.focus == group && GameUI.singleton?.spread){
                    GameUI.singleton.spread.Apply(cards);
                    return;
                }
            }

            for (var i = 0; i < cards.Count; i++)
            {
                var seekerSet = seekerSets?.GetSeekerSetFor(cards[i]);
                var heightOffset = MarginalCardOffset * i;
                var activeProxy = cards[i]?.activeProxy;
                if(activeProxy){
                    activeProxy.forceSeeking();
                    continue;
                }

                cards[i].Homing.StartSeeking(transform.position + Vector3.back * MountedCardAltitude + heightOffset, seekerSet?.Homing );
                if (Straighten)
                {
                    cards[i].Turning.StartSeeking(transform.rotation.eulerAngles.z, seekerSet?.Turning);
                }
                cards[i].Scaling.StartSeeking(UseMyScale ? groupScale : 1, seekerSet?.Scaling);
            }

            if (SecondaryCollider != null && cards.Count > 0)
            {
                SecondaryCollider.transform.position = transform.position + Vector3.forward + MarginalCardOffset * (cards.Count - 1);
            }
        }
    }
}
