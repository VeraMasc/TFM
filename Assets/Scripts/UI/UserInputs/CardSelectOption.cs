using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Globalization;


/// <summary>
/// Gestiona una opción concreta de CardSelectInput
/// </summary>
public class CardSelectOption : MonoBehaviour
{

    public CardSelectInput parent;
    public Card actualCard;
    public int index=-1;
    public bool selected;
    public SpriteRenderer outline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSourceCard(Card card){
        actualCard=card;
        useCardVisuals(card);
    }

    public void OnMouseDown()
    {
        if(selected){
            parent.chosen.Remove(index);
            
        }
        else{
            parent.chosen.Add(index);
        }
        selected = !selected;
        refreshOutlineColor(true);
    }

    public void refreshOutlineColor(bool hover=false){
         outline.color = selected? Colors.FromHex("F3EC7F"): 
            (hover?new Color(1,1,1,1):new Color(1,1,1,0));
    }

    public void OnMouseEnter()
    {
        outline.color = new Color(1,1,1,1);
    }

    public void OnMouseExit()
    {
        refreshOutlineColor();
    } 

    public void useCardVisuals(Card card){
        var visuals = card.FaceHoming.GetComponent<VisualsManager>();
        if(!visuals)
            return;
        visuals.cloneFace(transform);
        outline = transform.Find("Outline(Clone)")?.GetComponent<SpriteRenderer>();

        if(outline){
            outline.gameObject.SetActive(true);
            outline.color = new Color(1,1,1,0);
        }
        
    }
}



/// <summary>
/// Auxiliary class for color manipulation
/// </summary>
public class Colors
{
    public static Color FromHex(string hex)
    {
        if (hex.Length<6)
        {
            throw new System.FormatException("Needs a string with a length of at least 6");
        }

        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);
        string alpha;
        if (hex.Length >= 8)
            alpha = hex.Substring(6, 2);
        else
            alpha = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                        (int.Parse(g, NumberStyles.HexNumber) / 255f),
                        (int.Parse(b, NumberStyles.HexNumber) / 255f),
                        (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
    }
}