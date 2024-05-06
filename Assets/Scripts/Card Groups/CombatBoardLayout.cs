using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardHouse;

public class CombatBoardLayout : CardGroupSettings
{
    
    public int CardsPerColumn = 5;
    [Tooltip("Vertical offset")]
    public float MarginalCardOffset = 0.05f;

    /// <summary>
    /// Separación vertical base entre las cartas
    /// </summary>
    public float baseCardSpacing = 0.5f;
    Collider2D MyCollider;
    public bool Straighten = true;

    public override void Awake()
    {
        base.Awake();
        MyCollider = GetComponent<Collider2D>();
        if (MyCollider == null)
        {
            Debug.LogWarningFormat("{0}:{1} needs Collider2D on its game object to function.", gameObject.name, "GridLayout");
        }
    }

    protected override void ApplySpacing(List<Card> cards, SeekerSetList seekerSets)
    {
        var width = transform.lossyScale.x;
        var height = transform.lossyScale.y;

        var rowCount = CardsPerColumn;
        var realCardsPerRow = 1;

        int leftCount = 0;
        int rightcount = 0;

        foreach (var card in cards){

            var ownership =card.GetComponent<CardOwnership>();
            var isAllyOwned = ownership.controller.team == EntityTeam.player;
            int count = isAllyOwned? leftCount++:rightcount++;
            var newPos = transform.position
                + transform.right * width * (isAllyOwned? -0.5f: 0.5f)
                + transform.up * height * 0.5f
                + transform.up * (count + 1) * baseCardSpacing * -1
                + transform.forward * (MountedCardAltitude + MarginalCardOffset * count) * -1;

            var seekerSet = seekerSets?.GetSeekerSetFor(card);
                card.Homing.StartSeeking(newPos, seekerSet?.Homing);
                if (Straighten)
                {
                    card.Turning.StartSeeking(transform.rotation.eulerAngles.z, seekerSet?.Turning);
                }
                card.Scaling.StartSeeking(UseMyScale ? groupScale : 1, seekerSet?.Scaling);
        }

        
    }
}
