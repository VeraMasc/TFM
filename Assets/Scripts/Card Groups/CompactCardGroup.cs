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
        var n = Strategy.displayEveryN >0? Strategy.displayEveryN: int.MaxValue;
        var i = 0;
        foreach(var card in MountedCards){ //Iterate all cards
            if(card.Homing.seeking){ //Render Homing
                card.displayHiding(false);
            }
            else{
                //Is covering a previous card?
                if(mountedPrev!=null && mountedPrev.activeProxy==null){ 
                    mountedPrev.displayHiding(true);
                }
                
                if(i%n != 0)
                    mountedPrev=card; //Save as previous card
            }
            i++;
        }
    }

    private void OnMouseDown() {
        if( EventSystem.current?.IsPointerOverGameObject() ?? false)
            return;
        GameUI.setFocus(this);
        ApplyStrategy();
    }

}
