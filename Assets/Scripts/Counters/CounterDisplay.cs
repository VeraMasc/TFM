using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Se encarga de gestionar la representación de un contador concreto
/// </summary>
public class CounterDisplay : MonoBehaviour
{
    public TextMeshPro textBox;

    public new SpriteRenderer renderer;

    public void setCounter(string type, int value){
        renderer.color = getCounterColor(type);
        textBox.text = value.ToString();
    }

    public static Color getCounterColor(string type){
        if(typeColors.ContainsKey(type)){
            return typeColors[type];
        }
        return Color.grey;
    }

    public static Dictionary<string, Color> typeColors = new(){
        ["Duration"] =  Colors.FromHex("DAA520"),
        ["Use"] =  Colors.FromHex("228B22"),
        ["Bleed"] =  Colors.FromHex("8B0000"),
        ["Poison"] =  Colors.FromHex("ADFF2F"),
    };
}