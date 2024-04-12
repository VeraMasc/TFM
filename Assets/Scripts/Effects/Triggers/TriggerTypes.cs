using UnityEngine;

//[CreateAssetMenu(menuName = "Trigger/BaseTrigger")]
/// <summary>
/// Clase madre de todos los triggers
/// </summary>
public abstract class BaseTrigger :ScriptableObject
{
    public virtual void subscribe(){
        //TODO: implementar triggers con scriptable objects
    }
}

/// <summary>
/// Trigger de efecto global (afecta a todos los objetos con el trigger por igual)
/// </summary>
[CreateAssetMenu(menuName = "Trigger/GlobalTrigger")]
public class GlobalTrigger :BaseTrigger
{
    
}

/// <summary>
/// Trigger de efecto local (afecta solo a cartas concretas cada vez)
/// </summary>
[CreateAssetMenu(menuName = "Trigger/LocalTrigger")]
public class LocalTrigger :BaseTrigger
{
    
}