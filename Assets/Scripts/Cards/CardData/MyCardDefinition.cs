using System.Collections.Generic;
using System.Text.RegularExpressions;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.Serialization;
using System;


public class MyCardDefinition : CardDefinition
{
    public string cardName;

    [TextArea(3, 10),FormerlySerializedAs("cardText"),SerializeField]
    protected string rawText;

    [ReadOnly,TextArea(2, 6),]
    public string parsedText;

    [CustomInspector.Preview]
    public Sprite Art;

    public Sprite Frame;


    [ShowProperty("BackArt", order=-20),Space2(16)]

    [HorizontalLine]
    [SerializeReference,SubclassSelector]
    public List<CardHelp> help;

    /// <summary>
    /// Devuelve el tipo de la carta
    /// </summary>
    public virtual string getCardTypes(){
        return "";
    }
    protected virtual void Awake() {
        if(string.IsNullOrWhiteSpace(cardName)){
            cardName = name;
        }
    }

    public 
    void OnValidate() {
        parsedText = parseCardText(rawText,this);
    }

    /// <summary>
    /// Convierte el texto introducido en rich text
    /// </summary>
    /// <param name="raw"></param>
    /// <param name="definition"></param>
    /// <returns></returns>
    public static string parseCardText(string raw, MyCardDefinition definition){
        var parsed = raw;

        // Replace name
        parsed = Regex.Replace(parsed,@"(?<!\\)~", definition.cardName);

        //Replace costs 
        parsed = parseCost(parsed);
        
        //Replace links
        parsed = Regex.Replace(parsed,@"\{(?:(\S+?):\s*)?(.*?)\}", "<link=\"$1\">$2</link>");
        

        // Replace symbols
        parsed = Regex.Replace(parsed,@":: ", "• ");

        return parsed;
    }

    public static string parseCost(string raw){
        return Regex.Replace(raw,@"\{(\d+|\w)\}", 
            (x)=> $"<sprite name=\"{x.Groups[1].Value.ToUpper()}\">"
        );
    }
    
}