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

    public LocalTrigger onDeath;
    public LocalTrigger onDamage;
    public LocalTrigger onHeal;

    /// <summary>
    /// Trigger que se produce al inicio de cada turno del jugador propietario
    /// //TODO: Crear trigger "personal" (que agrupa los objetos por jugador)
    /// </summary>
    public LocalTrigger onBeginTurn;

    //TODO: Crear trigger "regional" (que agrupa los objetos por zona)
}