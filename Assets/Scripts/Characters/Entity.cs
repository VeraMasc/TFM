using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using CustomInspector;
using UnityEngine;
using Effect;
using Effect.Status;
using System.Linq;

/// <summary>
/// Cualquier entidad que usa cartas de acción
/// </summary>
public partial class Entity : MonoBehaviour, ITargetable
{

    /// <summary>
    /// IA que utiliza la entidad. Null si es controlado por el jugador
    /// </summary>
    public EntityAI AI;

    /// <summary>
    /// Contiene el maná que puede usar la entidad
    /// </summary>
    public ManaPool mana;

    /// <summary>
    /// Salud máxima del personaje
    /// </summary>
    public int maxHealth =20;

    /// <summary>
    /// Salud del personaje
    /// </summary>
    public int health =20;

    

    /// <summary>
    /// Indica si la entidad sigue con vida
    /// </summary>
    public bool alive = true;
    
    /// <summary>
    /// Equipo del personaje
    /// </summary>
    public EntityTeam team;

    /// <summary>
    /// Cantidad de cartas que puede tener en la mano al terminar el turno
    /// </summary>
    public int maxHandSize =4;

    [HorizontalLine(3f, message ="Card Management")]
    /// <summary>
    /// Mano con cartas de la entidad
    /// </summary>
    [ForceFill]
    public CardGroup hand;

    /// <summary>
    /// Mazo del personaje
    /// </summary>
    [ForceFill]
    public CardGroup deck;

    /// <summary>
    /// Pila de descarte del personaje
    /// </summary>
    [ForceFill]
    public CardGroup discarded;

    /// <summary>
    /// Pila de exilio del personaje
    /// </summary>
    [ForceFill]
    public CardGroup exile;

    /// <summary>
    /// Pila de cartas perdidas del personaje
    /// </summary>
    [ForceFill]
    public CardGroup lost;

    /// <summary>
    /// Pila de cartas enganchadas al personaje
    /// </summary>
    public CardGroup attached;

    /// <summary>
    /// Skills
    /// </summary>
    public CardGroup skills;

    /// <summary>
    /// Grupo de las cartas "en juego"
    /// </summary>
    public CardGroup board;

    [HorizontalLine(3f, message ="Data Management")]
    /// <summary>
    /// Definición del personaje
    /// </summary>
    public EntityData data;

    /// <summary>
    /// Estatus que actualmente afectan al personaje
    /// </summary>
    public List<Effect.Status.BaseStatus> statusEffects;

    [HorizontalLine(3f, message ="Interface")]

    /// <summary>
    /// Indica el punto en el que se pone el marcador de target
    /// </summary>
    [SerializeField]
    private Transform _targeterTransform;

    /// <summary>
    /// Indica el punto en el que se pone el marcador de target
    /// </summary>
    public Transform targeterTransform  {get=> _targeterTransform;}

    [SerializeField]
    private SpriteRenderer _outlineRenderer;

    public SpriteRenderer outlineRenderer {get=> _outlineRenderer;}

    /// <summary>
    /// Muestra la salud del personaje
    /// </summary>
    public HealthDisplay healthDisplay;


    /// <summary>
    /// Anctionables especiales (vienen de cartas que normalmente no se pueden usar por su zona)
    /// </summary>
    public List<IActionable> specialActionables;

    /// <summary>
    /// Obtiene todas las posibles acciones que puede tomar el jugador
    /// </summary>
    /// <returns></returns>
    public List<IActionable> getAllActionables(){
        var cardsInHand = hand.MountedCards?.Select(card => card.data)
                .OfType<IActionable>();

        var skillAbilities = skills.MountedCards?.Select(card => card.data)
                .OfType<SkillCard>()
                .SelectMany(skill => skill?.effects?.abilities)
                .OfType<IActionable>();

        var permanentAbilities = myPermanents
                .SelectMany(permanent => permanent?.effects?.abilities)
                .OfType<IActionable>();


        return cardsInHand.Concat(skillAbilities)
            .Concat(permanentAbilities)
            .Union(specialActionables).ToList();
    }

    public void executeAction(IActionable actionable){
        if(actionable is ActionCard card){

        }else if(actionable is ActivatedAbility ability){
            //TODO: Execute activated ability
        }
        else
        {
            Debug.LogError($"Actionable of type {actionable?.GetType()} is not implemented", this);
        }
    }

    void OnEnable()
    {
        //Add entity to list of entities
        GameController.singleton?.entities?.Add(this);
    }
    
    void OnDisable()
    {
        //Remove entity from list
        GameController.singleton?.entities?.Remove(this);
    }

    void Start()
    {
        initialize();
        onSceneChanged();
    }

    /// <summary>
    /// Inicializa los valores necesarios. Se realiza al crear la entidad por primera vez
    /// </summary>
    public void initialize(){
        health = maxHealth;
    }

    /// <summary>
    /// Inicializa los valores necesarios tras un cambio de escena
    /// </summary>
    public void onSceneChanged(){
        //Inicializa el mazo si no se ha hecho
        data.state ??= new DecklistState(data.decklist);

        //TODO: Cambiar lo que hace según si es modo combate o exploración

        StartCoroutine(DeckSetup.setupDeck(data.decklist, deck, this));
    }
}

[Serializable]
public enum EntityTeam{
    none,
    player,
    enemy
}