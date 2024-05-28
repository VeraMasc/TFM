using System;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Effect;
using Effect.Duration;
using UnityEngine;



/// <summary>
/// Añade una habilidad a una carta en cuestión
/// </summary>
[Serializable]
public abstract class AbilityModifier : BaseModifier
{
    [SerializeReference,SubclassSelector]
    public List<Ability> abilities;


}

/// <summary>
/// Añade una habilidad temporalmente
/// </summary>
[Serializable]
public class TemporaryAbilityModifier : BaseModifier
{
    [SerializeReference,SubclassSelector]
    public List<Ability> abilities;

    [SerializeReference,SubclassSelector]
    public IDuration duration;

    //TODO: complete 
}

