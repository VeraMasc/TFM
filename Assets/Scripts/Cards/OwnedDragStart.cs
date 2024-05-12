using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

using UnityEngine;

namespace CardHouse
{
    /// <summary>
    /// Impide arrastrar cartas que no están bajo tu control
    /// </summary>
    [RequireComponent(typeof(Card)), RequireComponent(typeof(DragOperator))]
    public class OwnedDragStart : Gate<NoParams>
    {
        Card MyCard;
        CardOwnership ownership;
        DragOperator MyDraggable;

        void Awake()
        {
            MyCard = GetComponent<Card>();
            MyDraggable = GetComponent<DragOperator>();
            ownership = GetComponent<CardOwnership>();
        }

        protected override bool IsUnlockedInternal(NoParams gateParams)
        {
            ownership ??= GetComponent<CardOwnership>();
            if(ownership){
                return ownership.owner.team == EntityTeam.player;
            }
            var owner = MyCard?.Group?.GetComponent<GroupZone>()?.owner;
            return owner != null && owner?.team == EntityTeam.player;
        }
    }
}

