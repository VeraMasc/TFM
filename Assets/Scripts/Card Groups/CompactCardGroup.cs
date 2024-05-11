using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using UnityEngine.EventSystems;
//TODO: Eliminar o replantear
/// <summary>
/// Grupo de cartas compacto
/// </summary>
public class CompactCardGroup : CardHouse.CardGroup
{
    
    /// <summary>
    /// Actualiza la visiblidad de las cartas del grupo. Solo se renderiza la carta superior o cartas en movimiento
    /// </summary>
    public void updateCardVisibility(){
        Card mountedPrev = null;
        foreach(var card in MountedCards){ //Iterate all cards
            if(card.Homing.seeking){ //Render Homing
                card.displayHiding(false);
            }
            else{
                if(mountedPrev!=null && mountedPrev.activeProxy==null){ //Is covering a previous card?
                    mountedPrev.displayHiding(true);
                }
                mountedPrev=card; //Save as previous card
            }
        }
    }

    private void OnMouseDown() {
        if( EventSystem.current?.IsPointerOverGameObject() ?? false)
            return;
        GameUI.setFocus(this);
        ApplyStrategy();
    }

}
