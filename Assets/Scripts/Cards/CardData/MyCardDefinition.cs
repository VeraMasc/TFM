using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;


public class MyCardDefinition : CardDefinition
{
    public string cardName;

    [TextArea(3, 10)]
    public string cardText;

    public Sprite Art;

    public Sprite Frame;

    [ShowProperty("BackArt", order=-20),Space2(16)]

    [HorizontalLine]
    [SerializeReference,SubclassSelector]
    public List<CardHelp> help;

    protected virtual void Awake() {
        if(string.IsNullOrWhiteSpace(cardName)){
            cardName = name;
        }
    }
}