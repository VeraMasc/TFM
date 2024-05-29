using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdvancedDragDetector : DragDetector
{
    public float dblClickCooldown=1f;
    float dblClickTimer;
    void OnMouseDown()
        {
            if (!IsActive || (EventSystem.current?.IsPointerOverGameObject() ?? false) || GameUI.singleton.activeUserInput)
                return; //Click desactivado

            

            if (!DragGates.AllUnlocked(null) ){ //Check for dblclick
                Card card = GetComponent<Card>();
                Debug.Log("Can't drag");
                if(dblClickTimer>0 &&  card.data is MyCardSetup setup)
                {
                    Debug.Log("Try activate");
                    
                    setup.tryActivateAsModal(GameMode.current.selectedPlayer);
                    // if(card.Group.GetComponent<GroupZone>()?.zone == GroupName.Board){
                    //     setup.tryActivateAsModal();
                    // }
                    // else{
                    //     setup.tryActivateAsModal();
                    // }
                    
                }
                dblClickTimer = dblClickCooldown;
                return;
            }
                
            
            if(dblClickTimer>0){
                var card = GetComponent<Card>();
                StartCoroutine(CardResolveOperator.singleton.playerUseCard(card));
                return;
            }
            dblClickTimer = dblClickCooldown;
            isDragging = true;
            OnDragStart.Invoke();
        }

        void OnMouseUp()
        {
            if (!IsActive || !DragGates.AllUnlocked(null))
                return;
            isDragging = false;
            OnDragEnd.Invoke();
        }


    void Update()
    {
        if(dblClickTimer>0){
            dblClickTimer -= Time.deltaTime;
        }
    }
}
