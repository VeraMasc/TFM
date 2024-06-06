using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Define las propiedades que tiene cada carta de Habilidad
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Skill")]
public class SkillCardDefinition : MyCardDefinition
{


    

    [SerializeField]
    public List<ActionSubtypes> typeList;


    
}

public enum SkillTypes{
    Core,
    Class,
    Innate
}

