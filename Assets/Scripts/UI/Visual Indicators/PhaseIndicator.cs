using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseIndicator : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var index = (int)CombatController.singleton.currentPhase+1;
        spriteRenderer.sprite = sprites[index];
    }
}
