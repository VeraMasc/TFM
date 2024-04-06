using System;
using System.Collections.Generic;
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

        public static Action<Card> OnCardFocused;

        /// <summary>
        /// Contiene el cardgroup de cartas asociadas (si existe)
        /// </summary>
        [CustomInspector.ReadOnly]
        public CardGroup attachedGroup;

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
            Group.UnMount(this);
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
            FaceHoming.StartSeeking(isFocused ?  new Vector3(0,0.5f,-2)
                    : Vector3.zero, useLocalSpace: true);
            FaceTurning.StartSeeking(isFocused ? Camera.main.transform.rotation.eulerAngles.z : 0, useLocalSpace: !isFocused);
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
        }

        public void TriggerUnMountEvents(GroupName group)
        {
            foreach (var eventTransition in GroupTransitionEvents)
            {
                if (eventTransition.Group == group)
                {
                    eventTransition.ExitEvent.Invoke();
                    break;
                }
            }
        }
        /// <summary>
        /// Evento de finalización del homing seeker 
        /// </summary>
        public void OnFinishHoming(){
            if(Group?.Strategy?.compactDisplay == true){
                //Actualiza la visiblidad de las cartas al acabar de colocar una
                Assert.IsTrue(Group is CompactCardGroup, "Compact display rquiere Compact Card Group");
                (Group as CompactCardGroup).updateCardVisibility();
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
