using UnityEngine;



/// <summary>
/// Define las propiedades que tiene cada carta de Habilidad
/// </summary>
[CreateAssetMenu(menuName = "Cards/Card Definition/Skill")]
public class SkillCardDefinition : MyCardDefinition
{


    [SerializeField]
    public BaseCardEffects effects;

    
}

