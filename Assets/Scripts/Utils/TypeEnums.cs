using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using System.Linq;
using System;


/// <summary>
/// Card Type enums
/// </summary>
/// <typeparam name="T"></typeparam>
public struct CardTypeEnum
{
    public string value;

    public static List<string> options = new(){
        "Blessing",
        "Hex",
        "Mental",
        "Meele",
        "Enchantment",
        "Aura",
    };

     public static Dictionary<string,List<string>> superTypes = new(){
        {"Enchantment", new(){"Blessing", "Hex"} }
    };

    public override bool Equals(object obj)
    {
        return value.Equals(obj);
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return value;
    }

    /// <summary>
    /// Custom equality operator
    /// </summary>
    public static bool operator ==(CardTypeEnum s1, CardTypeEnum s2) {
        
        return s1.value == s2.value;
    }
   
    /// <summary>
    /// Custom inequality operator
    /// </summary>
    public static bool operator !=(CardTypeEnum s1, CardTypeEnum s2) {
        return s1.value != s2.value;
    }

    /// <summary>
    /// Custom  operator (is within or equal)
    /// </summary>
    public static bool operator <=(CardTypeEnum s1, CardTypeEnum s2) {
        return s1 == s2 || s1 < s2;
    }

    /// <summary>
    /// Custom operator (contains or equal)
    /// </summary>
    public static bool operator >=(CardTypeEnum s1, CardTypeEnum s2) {
        return s1 == s2 || s1 > s2;
    }

    /// <summary>
    /// Custom operator (is within)
    /// </summary>
    public static bool operator <(CardTypeEnum s1, CardTypeEnum s2) {
        //Try get supertype
        if(superTypes.TryGetValue(s2.value, out var strings)){
            foreach(var str in strings){
                if(s1.value == str)
                    return true;
                //Go deeper
                var strType = new CardTypeEnum(){value=str};
                return s1 < strType; //Recur subtypes
            }
        }
        return false;
    }

    /// <summary>
    /// Custom operator (contains)
    /// </summary>
    public static bool operator >(CardTypeEnum s1, CardTypeEnum s2) {
        return s2 < s1;
    }
}