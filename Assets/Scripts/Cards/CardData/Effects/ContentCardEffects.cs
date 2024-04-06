using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;

using CustomInspector;
using System;

 
[Serializable]
public class ContentCardEffects:BaseCardEffects{
    /// <summary>
    /// Efectos que producir cuando la carta se revela
    /// </summary>
    public EffectChain revealEffect;


}


