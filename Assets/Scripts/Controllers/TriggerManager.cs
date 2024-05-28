using UnityEngine;
using Unity;
using Effect;
using CustomInspector;


/// <summary>
/// Se encarga de gestionar las referencias a los distintos triggers
/// </summary>
[CreateAssetMenu(menuName = "Trigger/TriggerManager")]
public class TriggerManager : ScriptableObject
{
   
    /// <summary>
    /// Trigger que se produce cuando una carta es revelada
    /// </summary>
    [Tab("Exploration")]
    public LocalTrigger onReveal;
    /// <summary>
    /// Trigger que se produce al visitar una habitación (antes del combate)
    /// </summary>
    [Tab("Exploration")]
    public LocalTrigger onVisit;

    /// <summary>
    /// Trigger que se produce tras visitar una habitación
    /// </summary>
    [Tab("Exploration")]
    public LocalTrigger onAfterVisit;

    [HorizontalLine(3,message = "Life")]
    [Tab("Entities")]
    public LocalTrigger onDeath;
    [Tab("Entities")]
    public LocalTrigger onDamage;
    [Tab("Entities")]
    public LocalTrigger onHeal;
    /// <summary>
    /// Se activa cuando una entidad usa una carta
    /// </summary>
    [Tab("Entities")]
    public GlobalTrigger onUseAction;

    /// <summary>
    /// Trigger producido al entrar la carta en el campo
    /// </summary>
    [HorizontalLine(3,message = "ZoneChanges")]
    [Tab("Zones")]
    public LocalTrigger onEnter;

    /// <summary>
    /// Se activa al destruir el objeto
    /// </summary>
    [Tab("Zones")]
    public LocalTrigger onDestroy;

    /// <summary>
    /// Se activa cuando el objeto deja una zona
    /// </summary>
    [Tab("Zones")]
    public LocalTrigger onLeave;

    [HorizontalLine(3,message = "Other Zones")]

    /// <summary>
    /// Se activa cuando el objeto deja el campo
    /// </summary>
    [Tab("Zones")]
    public LocalTrigger onEnterDiscard;

    /// <summary>
    /// Se activa cuando la carta se exilia
    /// </summary>
    [Tab("Zones")]
    public LocalTrigger onEnterExile;

    /// <summary>
    /// Se activa cuando la carta se "pierde"
    /// </summary>
    [Tab("Zones")]
    public LocalTrigger onEnterLost;

    /// <summary>
    /// Se activa al lanzar esta carta
    /// </summary>
    [HorizontalLine(3,message = "Actions")]
    [Tab("Cards")]
    public LocalTrigger onCast;


    /// <summary>
    /// Se activa al resolverse la carta
    /// </summary>
    [Tab("Cards")]
    public LocalTrigger onSpent;

    

    /// <summary>
    /// Trigger que se produce al inicio de cada turno del jugador propietario
    /// //TODO: Crear trigger "personal" (que agrupa los objetos por jugador)
    /// </summary>
    [HorizontalLine(3,message = "Timing")]

    [Tab("Phases")]
    public GlobalTrigger beforeBeginTurn;
    [Tab("Phases")]
    public GlobalTrigger onBeginTurn;

    [Tab("Phases")]
    public GlobalTrigger onMainPhase;
    [Tab("Phases")]
    public GlobalTrigger onEndTurn;
    [Tab("Phases")]
    public GlobalTrigger afterEndTurn;

    //TODO: Crear trigger "regional" (que agrupa los objetos por zona)
}