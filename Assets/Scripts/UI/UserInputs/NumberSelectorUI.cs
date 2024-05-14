using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberSelectorUI : MonoBehaviour
{
    public TextMeshProUGUI display;
    public int currentValue;

    /// <summary>
    /// Rango de valores aceptado //TODO: implementar
    /// </summary>
    public Vector2 range;

    void Start()
    {
        refresh();
    }
    
    [NaughtyAttributes.Button]
    public void refresh(){
        display.text = currentValue.ToString();
    }

    public void increase(){
        currentValue++;
        refresh();
    }

    public void decrease(){
        currentValue--;
        refresh();
    }
}
