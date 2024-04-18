using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardHouse
{
    public class SpreadLayout : CardGroupSettings
    {
        /// <summary>
        /// Rect del objeto que simula el grupo esparcido
        /// </summary>
        private RectTransform rect;

        /// <summary>
        /// Cuanto incrementa la escala de las cartas centrales
        /// </summary>
        public float scaleBoost;

        /// <summary>
        /// Tamaño de la zona en la que se resaltan las cartas
        /// </summary>
        public float highlightSize = 3f;
        /// <summary>
        /// Tamaño del spread en número de cartas
        /// </summary>
        public int size;

        
        /// <summary>
        /// Espacio de separación entre carta y carta
        /// </summary>
        public float cardSeparation = 1f;
        public float MarginalCardOffset = 0.01f;
        public Vector2 ArcCenterOffset = new Vector2(0f, -5f);
        [Range(0f, 0.8f)]
        Collider2D MyCollider;

        void Awake()
        {
            MyCollider = GetComponent<Collider2D>();
            if (MyCollider == null)
            {
                Debug.LogWarningFormat("{0}:{1} needs Collider2D on its game object to function.", gameObject.name, "SplayLayout");
            }
            rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            ArcCenterOffset = transform.position + transform.right * ArcCenterOffset.x + transform.up * ArcCenterOffset.y;
        }

        protected override void ApplySpacing(List<Card> cards, SeekerSetList seekerSets = null)
        {
            if(cards.Count <= 1)
                return;
            size = cards.Count;
            var width = size*cardSeparation;
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y); //Ajustar rect

            var spacing = cardSeparation;
            for (var i = 0; i < cards.Count; i++)
            {
                
                var newPos = transform.position

                             + transform.right * -width 
                             + transform.right * (i + 1) * spacing;

                var offCenter = newPos.x -ArcCenterOffset.x;
                newPos+= Vector3.back * (MountedCardAltitude + (offCenter/spacing) * MarginalCardOffset); //Keep center at 0
                var seekerSet = seekerSets?.GetSeekerSetFor(cards[i]);
                cards[i].Homing.StartSeeking(newPos, seekerSet?.Homing);

                
                var centerness = Mathf.Max(highlightSize - Mathf.Abs(offCenter), 0)/highlightSize;
                if(Math.Abs(offCenter) > highlightSize){
                    offCenter -= highlightSize * Math.Sign(offCenter);
                }
                else{ //ZOna de planitud
                    offCenter=0;
                }
                var newAngle = Mathf.Atan2(newPos.y - ArcCenterOffset.y, offCenter) * Mathf.Rad2Deg - 90;
                newAngle = Mathf.Clamp(newAngle,-70, 70);
                cards[i].Turning.StartSeeking(newAngle, seekerSet?.Turning);

                
                var boost= centerness * scaleBoost;
                cards[i].Scaling.StartSeeking((UseMyScale ? groupScale : 1) + boost, seekerSet?.Scaling);
            }
        }
    }
}
