using System;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;

namespace CardHouse
{
    /// <summary>
    /// Layout que distribuye las cartas en la mano
    /// </summary>
    public class HandLayout : CardGroupSettings
    {
        /// <summary>
        /// Invertir el orden de las cartas en mano (por defecto izquierda a derecha)
        /// </summary>
        public bool invertOrder;

        public bool selected;
        public float MarginalCardOffset = 0.01f;
        public Vector2 ArcCenterOffset = new Vector2(0f, -5f);
        [Range(0f, 0.8f)]
        public float ArcMargin = 0.3f;
        Collider2D MyCollider;

        public float selectedScale=1.5f;

        //Offset vertical de las cartas seleccionadas
        public float selectedHeight=1.5f;

        public float selectedWidthFactor=2f;

        public float handProxySeparation = 0.4f;

        /// <summary>
        /// Lista de proxies que mostrar en la mano
        /// </summary>
        public List<CardProxy> proxies = new();

        public override void Awake()
        {
            base.Awake();
            MyCollider = GetComponent<Collider2D>();
            if (MyCollider == null)
            {
                Debug.LogWarningFormat("{0}:{1} needs Collider2D on its game object to function.", gameObject.name, "SplayLayout");
            }
        }

        private void Start()
        {
            
        }

        protected override void ApplySpacing(List<Card> cards, SeekerSetList seekerSets = null)
        {
            var width = transform.lossyScale.x * (1f - ArcMargin);
            if(selected)
                width *= selectedWidthFactor;

            var numProxies = proxies.Count;
            var widthReduction = numProxies>0? handProxySeparation:0;
            var spacing = (width-widthReduction) / (cards.Count + numProxies + 1);
            var xScaleSign = Math.Sign(transform.lossyScale.x);
            var xAxis = transform.right*xScaleSign;
            
            var rootPos = selected? GameUI.singleton.handDetails.position :transform.position;

            if(numProxies>0){
                    rootPos+= (-xAxis) * handProxySeparation/2;
            }
            for (var i = 0; i < cards.Count; i++)
            {
                
                var direction = invertOrder? transform.right:-transform.right;
                var newPos = rootPos
                             + Vector3.back * (MountedCardAltitude + (cards.Count-i) * MarginalCardOffset)
                             + direction * width * -0.5f
                             + direction * (i + numProxies + 1) * spacing;

                if(selected)
                    newPos += selectedHeight * Vector3.back;
                
                var seekerSet = seekerSets?.GetSeekerSetFor(cards[i]);
                cards[i].Homing.StartSeeking(newPos, seekerSet?.Homing);

                var realCenterOffset = (Vector2)rootPos + ArcCenterOffset * (selected? selectedWidthFactor:1);
                
                var newAngle = Mathf.Atan2(newPos.y - realCenterOffset.y, newPos.x - realCenterOffset.x) * Mathf.Rad2Deg - 90;
                cards[i].Turning.StartSeeking(newAngle, seekerSet?.Turning);

                var scale = selected? selectedScale:(UseMyScale ? groupScale : 1);

                cards[i].Scaling.StartSeeking(scale, seekerSet?.Scaling);

            }

            var newRootpos = rootPos + Vector3.back * cards.Count * MarginalCardOffset;
            handleProxies(newRootpos,width, spacing);
        }

        public void handleProxies(Vector3 rootPos,float width,float spacing){
            var i=0;
            var xScaleSign = Math.Sign(transform.lossyScale.x);
            var xAxis = transform.right*xScaleSign;

            rootPos += xAxis * handProxySeparation;
            
            foreach(var proxy in proxies.Where(p => p?.isActiveProxy == true)){

                var direction = invertOrder? transform.right:-transform.right;
                var newPos = rootPos
                             + Vector3.back * (MountedCardAltitude + (proxies.Count-i) * MarginalCardOffset)
                             + direction * width * -0.5f
                             + direction * (i + 1) * spacing;
                proxy.Homing.StartSeeking(newPos, null);
                proxy.Turning.StartSeeking(0, null);
                var scale = selected? selectedScale:(UseMyScale ? groupScale : 1);
                proxy.Scaling.StartSeeking(scale, null);

                if(proxy){
                    UCoroutine.Yield(new WaitForFixedUpdate())
                    .Then(()=>proxy.forceSeeking())
                    .Start(proxy);
                }
                i++;
            }
        }
        
    }

}
