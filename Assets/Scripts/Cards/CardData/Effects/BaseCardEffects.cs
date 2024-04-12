
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Effect;
using UnityEngine;



[Serializable]
public class BaseCardEffects{
    /// <summary>
    /// Efectos que producir cuando la carta se "resuelve" (Esto significa cosas distintas dependiendo del tipo de carta)
    /// </summary>
    public EffectChain baseEffect;

    /// <summary>
    /// Lista de habilidades de la carta
    /// </summary>
    [SerializeReference, SubclassSelector]
    public List<Ability> abilities;

    /// <summary>
    /// Efectos estáticos 
    /// </summary>
    public EffectChain staticEffects;

    public Effect.Context context = null;

    /// <summary>
    /// Clona todos los efectos del conjunto
    /// </summary>
    /// <returns></returns>
    public  BaseCardEffects cloneAll(){
        using (MemoryStream stream = new MemoryStream())
        {
            if (this.GetType().IsSerializable)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Position = 0;
                return formatter.Deserialize(stream) as BaseCardEffects;
            }
            return null;
        }
    }

}


