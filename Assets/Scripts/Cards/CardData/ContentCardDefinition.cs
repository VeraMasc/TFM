using UnityEngine;
using CardHouse;
using CustomInspector;

using UnityEditor;
using System.IO;


/// <summary>
/// Define las propiedades que tiene cada carta de habitación
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Content")]
public class ContentCardDefinition : MyCardDefinition
{


    [SerializeField]
    public ContentCardEffects effects;

    protected override void Awake() {
        base.Awake();
        // effects = (ContentCardEffects)effects.cloneAll();
    }
}

