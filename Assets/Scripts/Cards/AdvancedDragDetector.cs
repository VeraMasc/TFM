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
            if (!IsActive || !DragGates.AllUnlocked(null) || (EventSystem.current?.IsPointerOverGameObject() ?? false) || GameUI.singleton.activeUserInput)
                return;
            
            if(dblClickTimer>0){
                var card = GetComponent<Card>();
                Debug.Log("Dbl click");
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
