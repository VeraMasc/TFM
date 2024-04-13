using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using UnityEngine.Events;

//[CreateAssetMenu(menuName = "Trigger/BaseTrigger")]
/// <summary>
/// Clase madre de todos los triggers
/// </summary>
public abstract class BaseTrigger :ScriptableObject
{
    public UnityEvent triggerEvent;
    
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
    public HashSet<Component> subscribers = new HashSet<Component>();
    public virtual void subscribe(Component subscriber){
        if(subscribers.Contains(subscriber)){
            return;
        }
        //TODO: implementar triggers con scriptable objects
    }
    public virtual void unsubscribe(Component subscriber){
        if(subscribers.Contains(subscriber)){
            
            // triggerEvent.RemoveListener
            // return;
        }
        
    }
}