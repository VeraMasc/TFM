using System;
using System.Collections.Generic;
using System.Linq;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace CardHouse
{
    [RequireComponent(typeof(Homing)), RequireComponent(typeof(Turning)), RequireComponent(typeof(Scaling))]
    public class Card : MonoBehaviour, ITargetable
    {
        [Serializable]
        public class GroupTransitionEvent
        {
            public GroupName Group;
            public UnityEvent EntryEvent;
            public UnityEvent ExitEvent;
        }

        [ReadOnly]
        public CardGroup Group;
        /// <summary>
        /// Object that holds all the card data
        /// </summary>
        public CardSetup data;
        public Homing Homing { get; private set; }
        public Turning Turning { get; private set; }
        public Scaling Scaling { get; private set; }

        public Animator FlipAnimator;

        [CustomInspector.ShowMethod("isFaceUp")]
        public bool CanBeUpsideDown;
        [Range(0f, 1f)]
        public float UpsideDownChance = 0.5f;
        public Transform RootToRotateWhenUpsideDown;

        /// <summary>
        /// Indica el punto en el que se pone el marcador de target
        /// </summary>
        [SerializeField]
        private Transform _targeterTransform;

        /// <summary>
        /// Indica el punto en el que se pone el marcador de target
        /// </summary>
        public Transform targeterTransform  {get=> _targeterTransform;}

        [SerializeField]
        private SpriteRenderer _outlineRenderer;

        public SpriteRenderer outlineRenderer {get=> _outlineRenderer;}

        public Homing FaceHoming;
        public Turning FaceTurning;
        public Scaling FaceScaling;

        public List<GroupTransitionEvent> GroupTransitionEvents;
        
        public CardFacing Facing { get { return FlipAnimator.GetBool("FaceUp") ? CardFacing.FaceUp : CardFacing.FaceDown; } }

        public UnityEvent OnFlipUp;
        public UnityEvent OnFlipDown;
        public UnityEvent OnPlay;

        public Action<Card, CardGroup> OnMount;

        bool IsFocused;

        /// <summary>
        /// Proxies de la carta en cuestión
        /// </summary>
        public List<CardProxy> proxies;

        /// <summary>
        /// Proxy that is currently active
        /// </summary>
        public CardProxy activeProxy => proxies?.Where(p =>p.isActiveProxy)
                    ?.FirstOrDefault();

        public static Action<Card> OnCardFocused;

        /// <summary>
        /// Contiene el cardgroup de cartas asociadas (si existe)
        /// </summary>
        [CustomInspector.ReadOnly]
        public CardGroup attachedGroup;

        /// <summary>
        /// Card ownership (la recupera si no está asignada)
        /// </summary>
        public CardOwnership ownership => _ownership ??= GetComponent<CardOwnership>();

        private CardOwnership _ownership;

        void Awake()
        {
            Homing = GetComponent<Homing>();
            Turning = GetComponent<Turning>();
            Scaling = GetComponent<Scaling>();
            OnCardFocused += HandleCardFocused;
            data = GetComponent<CardSetup>();
        }

        void OnDestroy()
        {
            OnCardFocused -= HandleCardFocused;
        }

        public void DestroyCard(){
            if(attachedGroup != null){
                //TODO: Destroygroup default pile
                attachedGroup.destroyGroup(GroupName.Discard);    
            }
            Group?.UnMount(this);
            Destroy(this.gameObject);
        }

        void Update()
        {
            if (IsFocused && Input.GetMouseButtonDown(0))
            {
                SetFocus(false);
            }
        }

        public bool isFaceUp(){
            return FlipAnimator.isInitialized && Facing == CardFacing.FaceUp;
        }
        public void SetFacing(bool isFaceUp)
        {
            SetFacing(isFaceUp ? CardFacing.FaceUp : CardFacing.FaceDown);
        }

        public void SetFacing(CardFacing facing, bool immediate = false, float spd = 1f)
        {
            if (facing == CardFacing.None)
                return;
            
            FlipAnimator.SetBool("SkipAnimation", immediate);
            FlipAnimator.SetBool("FaceUp", facing == CardFacing.FaceUp);
            FlipAnimator.speed = spd;

            if (facing == CardFacing.FaceUp)
            {
                OnFlipUp?.Invoke();
            }
            else if (facing == CardFacing.FaceDown)
            {
                OnFlipDown?.Invoke();
            }
        }

        public void SetUpsideDown(bool isUpsideDown)
        {
            if (!CanBeUpsideDown)
                return;

            var currentRotation = RootToRotateWhenUpsideDown.localRotation.eulerAngles;
            currentRotation += Vector3.forward * ((isUpsideDown ? 180f : 0f) - RootToRotateWhenUpsideDown.localRotation.eulerAngles.z);

            RootToRotateWhenUpsideDown.localRotation = Quaternion.Euler(currentRotation);
        }

        public bool IsUpsideDown => CanBeUpsideDown && (Mathf.Abs(RootToRotateWhenUpsideDown.localRotation.eulerAngles.z) - 180f) < 1f;

        public void HandlePlayed()
        {
            OnPlay.Invoke();
        }

        public CardGroup GetDiscardGroup()
        {
            var ownerIndex = GroupRegistry.Instance.GetOwnerIndex(Group);
            if (ownerIndex == null && GetComponent<CardLoyalty>() != null)
            {
                ownerIndex = GetComponent<CardLoyalty>().PlayerIndex;
            }
            var discardGroup = GroupRegistry.Instance?.Get(GroupName.Discard, ownerIndex);
            return discardGroup;
        }

        /// <summary>
        /// Pone el foco en la imagen de la carta
        /// </summary>
        /// <param name="isFocused"></param>
        public void SetFocus(bool isFocused)
        {
            IsFocused = isFocused;

            var zone = Group?.GetComponent<GroupZone>()?.zone;
            if(zone == GroupName.Stack){
                isFocused = false;
            }

            var isHand = zone == GroupName.Hand || (activeProxy && !Group.isFocused);

            var targetPos =  Vector3.zero;
            if(isFocused){
                targetPos += new Vector3(0,0,-2);

                if(isHand){
                    targetPos += new Vector3(0,0.9f,0);
                }
            }

            FaceHoming.StartSeeking(targetPos, useLocalSpace: true);
            FaceTurning.StartSeeking(isFocused && isHand? Camera.main.transform.rotation.eulerAngles.z : 0, useLocalSpace: !isFocused);
            FaceScaling.StartSeeking(isFocused ? 1.5f: 1f, useLocalSpace: true);

            if (isFocused)
            {
                OnCardFocused?.Invoke(this);
            }
        }

        public void onRightClick(){
            if(Facing != CardFacing.FaceUp)
                return;
                
            Instantiate(FaceHoming.transform.GetChild(0),GameUI.singleton.cardDetails.transform);
            
        }

        void HandleCardFocused(Card card)
        {
            if (IsFocused && card != this)
            {
                SetFocus(false);
            }
        }

        public void ToggleFocus()
        {
            SetFocus(!IsFocused);
        }

        public void TriggerMountEvents(CardGroup group)
        {
            OnMount?.Invoke(this, group);

            var groupName = GroupRegistry.Instance?.GetGroupName(group) ?? GroupName.None;
            foreach (var eventTransition in GroupTransitionEvents)
            {
                if (eventTransition.Group == groupName)
                {
                    eventTransition.EntryEvent?.Invoke();
                    break;
                }
            }
            //Eliminar outline al mover la carta
            outlineRenderer?.gameObject?.SetActive(false);
            
            if(this.data is MyCardSetup setup){
                var zone = group?.GetComponent<GroupZone>();
                CardResolveOperator.singleton.stackUI.refresh();
                var afterAnimation = UCoroutine.YieldAwait( ()=>!Homing.seeking);
                
                
                
                //Si ha cambiado de zona...
                if(zone.zone != setup.effects.sourceZone){
                    if(zone.zone != GroupName.Board)
                        CounterHolder.getHolder(this)?.clear(); ///Clear counters
                    
                    if(zone.zone == GroupName.Stack){
                        afterAnimation = afterAnimation
                            .Then(CardResolveOperator.singleton.precalculateCard(this));
                    }
                    else{ //Activar modifiers si los hay
                        var zoneMods = CardModifiers.getModifiers(this).OfType<ForceZoneModifier>();
                        if(zoneMods.Select(m =>m.activate()).Any(v => v)){
                            return; //No activar el resto de efectos si al menos un modificador ha tenido éxito
                        }
                    }
                    //Llamar triggers
                    afterAnimation = afterAnimation
                    .Then(zone.callLeaveTrigger(this))
                    .Then(zone.callEnterTrigger(this)) //Call before refreshing subscriptions
                    .Then(()=>{
                        setup.GetComponent<CardModifiers>()?.refreshModifiers();
                        //Update ability subscriptions
                        setup.effects.refreshAbilitySuscriptions(zone.zone);
                    });
                }
                afterAnimation.Start(this);
            }
            
        }

        public void TriggerUnMountEvents(CardGroup group)
        {
            GroupName groupName = GroupRegistry.Instance?.GetGroupName(group) ?? GroupName.None;

            foreach (var eventTransition in GroupTransitionEvents)
            {
                if (eventTransition.Group == groupName)
                {
                    eventTransition.ExitEvent.Invoke();
                    break;
                }
            }

            if(this.data is MyCardSetup setup){
                var zone = group?.GetComponent<GroupZone>();

                //Cambiar el contexto de la carta
                setup.effects.sourceZone = zone?.zone ?? GroupName.None;
                setup.effects.sourceGroup = group;
                setup.effects.entryTime = Time.timeAsDouble;
                setup.effects.context.precalculated=false;

                //Eliminar texto temporal
                setup.tempText ="";
                setup.applyText();
            }
        }
        /// <summary>
        /// Evento de finalización del homing seeker 
        /// </summary>
        public void OnFinishHoming(){
            if(Group?.Strategy?.compactDisplay == true && GameUI.focus != Group && !activeProxy){
                //Actualiza la visiblidad de las cartas al acabar de colocar una
                Assert.IsTrue(Group is CompactCardGroup, "Compact display rquiere Compact Card Group");
                (Group as CompactCardGroup).updateCardVisibility();
                var col = GetComponent<Collider2D>();
                if(col){
                    col.enabled = false;
                }
            }
        }

        /// <summary>
        /// Permite desactivar o activar los elementos de display de la carta para malgastar menos recursos
        /// </summary>
        /// <param name="hide">True si se quiere desactivar el display</param>
        public void displayHiding(bool hide){
            //transform.GetChild(0).gameObject.SetActive(!hide);
            FaceHoming.SendMessage("hideVisuals", hide, SendMessageOptions.DontRequireReceiver);
            // var sprites = FaceHoming.transform.GetComponentsInChildren<SpriteRenderer>(true);
            // //TODO: optimize
            // foreach(var sprite in sprites){
            //     sprite.gameObject.SetActive(!hide);
            // }
            
        }
    }
}
