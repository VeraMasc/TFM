using System;
using UnityEngine;

namespace Effect{
public class ManaCost:ICost
{
    /// <summary>
    /// Coste en formato de texto
    /// </summary>
    public string costText;

    /// <summary>
    /// Comprueba que el coste tiene sentido
    /// </summary>
    /// <returns></returns>
    public bool isValid(){
        return false;
    }

    /// <summary>
    /// Checks if the cost can be paid
    /// </summary>
    public bool canBePaid(Effect.Context context){
        //TODO: Implement mana costs
        throw new NotImplementedException();
    }
}

}