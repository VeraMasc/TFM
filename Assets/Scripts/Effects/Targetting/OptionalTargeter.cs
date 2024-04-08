using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Gestiona la existencia de targets opcionales
/// </summary>
[Serializable]
public class OptionalTargeter:EffectTargeter
{
    //TODO: Optional targeting
    public ContextualObjTargets contextual;
    public override void resolveTarget(Effect.Context context){
        
    }

}