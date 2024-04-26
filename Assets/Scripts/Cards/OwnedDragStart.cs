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
        DragOperator MyDraggable;

        void Awake()
        {
            MyCard = GetComponent<Card>();
            MyDraggable = GetComponent<DragOperator>();
        }

        protected override bool IsUnlockedInternal(NoParams gateParams)
        {
            var owner = MyCard?.Group?.GetComponent<GroupOwner>()?.owner;
            return owner != null && owner?.team == EntityTeam.player;
        }
    }
}

