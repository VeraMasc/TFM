using UnityEngine;
using CardHouse;
using TMPro;
using NaughtyAttributes;
using System.Collections.Generic;

using CustomInspector;
using System;

 
[Serializable]
public class RoomCardEffects:BaseCardEffects{
    /// <summary>
    /// Efectos que producir cuando se escoge la habitación
    /// </summary>
    public EffectChain onVisitEffect;


    /// <summary>
    /// Se activa tras visitar la habitación, terminar el combate (si lo hay) y antes de ver las próximas habitaciones
    /// </summary>
    public EffectChain beforeLeavingEffect;

    /// <summary>
    /// Efectos que producir cuando la carta se revela
    /// </summary>
    public EffectChain revealEffect;
}


