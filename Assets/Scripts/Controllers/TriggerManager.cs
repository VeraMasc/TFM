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
    [HorizontalLine(3,message = "Exploration")]
    /// <summary>
    /// Trigger que se produce cuando una carta es revelada
    /// </summary>
    public LocalTrigger onReveal;
    /// <summary>
    /// Trigger que se produce al visitar una habitación (antes del combate)
    /// </summary>
    public LocalTrigger onVisit;

    /// <summary>
    /// Trigger que se produce tras visitar una habitación
    /// </summary>
    public LocalTrigger onAfterVisit;

    [HorizontalLine(3,message = "Life")]
    public LocalTrigger onDeath;
    public LocalTrigger onDamage;
    public LocalTrigger onHeal;

    /// <summary>
    /// Trigger producido al entrar la carta en el campo
    /// </summary>
    [HorizontalLine(3,message = "Board")]
    public LocalTrigger onEnter;

    /// <summary>
    /// Se activa al destruir el objeto
    /// </summary>
    public LocalTrigger onDestroy;

    /// <summary>
    /// Se activa cuando el objeto deja una zona
    /// </summary>
    public LocalTrigger onLeave;

    [HorizontalLine(3,message = "Other Zones")]

    /// <summary>
    /// Se activa cuando el objeto deja el campo
    /// </summary>
    public LocalTrigger onEnterDiscard;

    /// <summary>
    /// Se activa cuando la carta se exilia
    /// </summary>
    public LocalTrigger onEnterExile;

    /// <summary>
    /// Se activa cuando la carta se "pierde"
    /// </summary>
    public LocalTrigger onEnterLost;

    /// <summary>
    /// Se activa al lanzar esta carta
    /// </summary>
    [HorizontalLine(3,message = "Actions")]
    public LocalTrigger onCast;


    /// <summary>
    /// Se activa al resolverse la carta
    /// </summary>
    public LocalTrigger onSpent;

    /// <summary>
    /// Se activa al usar CUALQUIER carta
    /// </summary>
    public PersonalTrigger onUseAction;

    /// <summary>
    /// Trigger que se produce al inicio de cada turno del jugador propietario
    /// //TODO: Crear trigger "personal" (que agrupa los objetos por jugador)
    /// </summary>
    [HorizontalLine(3,message = "Timing")]
    public GlobalTrigger onBeginTurn;

    public GlobalTrigger onEndTurn;

    //TODO: Crear trigger "regional" (que agrupa los objetos por zona)
}