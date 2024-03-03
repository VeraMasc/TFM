using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace CardHouse
{
    /// <summary>
    /// Layout que distribuye las cartas en una espiral/escalera de caracol partiendo de una posición
    /// </summary>
    public class SpiralLayout : CardGroupSettings
    {
        /// <summary>
        /// Separación entre las distintas cartas
        /// </summary>
        [Hook(nameof(refreshMountedCards))]
        public float angularCardOffset = 5f;

        /// <summary>
        /// Separación en la altura de las cartas
        /// </summary>
        [Hook(nameof(refreshMountedCards))]
        public float MarginalCardOffset = 0.01f;

        /// <summary>
        /// Anchura del giro de la espiral
        /// </summary>
        [Hook(nameof(refreshMountedCards))]
        public float spiralWidth = 1f;

        Collider2D MyCollider;

        void Awake()
        {
            MyCollider = GetComponent<Collider2D>();
            if (MyCollider == null)
            {
                Debug.LogWarningFormat("{0}:{1} needs Collider2D on its game object to function.", gameObject.name, "SplayLayout");
            }
        }


        protected override void ApplySpacing(List<Card> cards, SeekerSetList seekerSets = null)
        {
            var basePos = transform.position + Vector3.back * MountedCardAltitude;
            var baseOffset = Vector3.back * MarginalCardOffset;
            var widthVector = Vector3.up * spiralWidth;
            for (var i = 0; i < cards.Count; i++)
            {
                var cardAngle = (cards.Count-i-1) * angularCardOffset + transform.rotation.eulerAngles.z;

                var cardArc =  Quaternion.AngleAxis(cardAngle,Vector3.forward);
                var newPos = basePos + i * baseOffset//Base position
                    + cardArc * widthVector //Circle arc
                    - widthVector;
                //  transform.position
                //              + Vector3.back * (MountedCardAltitude + i * MarginalCardOffset)
                //              + transform.right * width * -0.5f
                //              + transform.right * (i + 1) * spacing;

                var seekerSet = seekerSets?.GetSeekerSetFor(cards[i]);
                cards[i].Homing.StartSeeking(newPos, seekerSet?.Homing);

                cards[i].Turning.StartSeeking(cardAngle, seekerSet?.Turning);

                cards[i].Scaling.StartSeeking(UseMyScale ? transform.lossyScale.y : 1, seekerSet?.Scaling);
            }
        }

        
    }
}
