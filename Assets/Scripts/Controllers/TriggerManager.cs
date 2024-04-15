using UnityEngine;
using Unity;
using Effect;


/// <summary>
/// Se encarga de gestionar las referencias a los distintos triggers
/// </summary>
[CreateAssetMenu(menuName = "Trigger/TriggerManager")]
public class TriggerManager : ScriptableObject
{
    /// <summary>
    /// Trigger que se produce cuando una carta es revelada
    /// </summary>
    public LocalTrigger onReveal;
}