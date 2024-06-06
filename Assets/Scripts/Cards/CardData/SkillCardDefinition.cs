using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;



/// <summary>
/// Define las propiedades que tiene cada carta de Habilidad
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Skill")]
public class SkillCardDefinition : MyCardDefinition
{

    [SerializeField]
    public List<SkillTypes> typeList;

   

    public static string parseCardLevel(string raw, int level){
        var parsed = raw;
        parsed = Regex.Replace(parsed,@"{[Ll][Vv]}",level.ToString());
        return parsed;
    }
    
}

public enum SkillTypes{
    Core,
    Class,
    Innate
}

