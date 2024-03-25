using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using TMPro;
using UnityEngine;

/// <summary>
/// Refresca el indicador del tamazo de mazo cuando es necesario
/// </summary>
public class GroupSizeRefresher : MonoBehaviour
{

    /// <summary>
    /// Último valor registrado
    /// </summary>
    [ReadOnly]
    public int lastValue = -1;

    /// <summary>
    /// Grupo del que controla el valor
    /// </summary>
    public CardGroup parentGroup;

    /// <summary>
    /// Elemento que muestra el texto como tal
    /// </summary>
    [SelfFill]
    public TextMeshPro display;

    void OnEnable()
    {
        parentGroup ??= GetComponentInParent<CardGroup>();
        refresh();
    }
    // Update is called once per frame
    void Update()
    {
        refresh();
    }

    /// <summary>
    /// Actualiza el valor representado
    /// </summary>
    public void refresh(){
        if(parentGroup.MountedCards.Count != lastValue){
            lastValue = parentGroup.MountedCards.Count;
            display.text = lastValue.ToString();
        }
    }
}
