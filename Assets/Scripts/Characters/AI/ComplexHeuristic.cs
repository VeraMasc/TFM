using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Effect;
using UnityEngine;

/// <summary>
/// Define heurísticas para cartas con varias partes
/// </summary>
[Serializable]
public class ComplexHeuristic:CardHeuristic,ITargetingHeuristic,ITimingHeuristic{
    
    public List<ChoiceHeuristic> choices;

    public override void validate(){
        foreach(var choice in choices){
            if (string.IsNullOrEmpty(choice?.choicePath))
                continue;
            var split = Regex.Split(choice.choicePath,@",");
            choice.choicePathArray = split
                .Select(s => int.TryParse(s, out int v)?(int?)v:null)
                .Where(i => i!=null)
                .Select(i => i.Value)
                .ToArray();
        }
    }
}

[Serializable]
public class ChoiceHeuristic{
    public string choicePath;
    [CustomInspector.ReadOnly]
    public int[] choicePathArray;
    [SerializeReference, SubclassSelector]
    public CardHeuristic heuristic;
}

