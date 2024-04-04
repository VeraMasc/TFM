using UnityEngine;

/// <summary>
/// Contexto que se usa para resolver targets y efectos
/// </summary>
public class TargettingContext 
{
    /// <summary>
    /// Refiere al propio objeto que produce el efecto
    /// </summary>
    public GameObject self;

    /// <summary>
    /// Refiere a la fuente que causa el efecto
    /// (ES NECESARIO???)
    /// </summary>
    public GameObject source;

    /// <summary>
    /// Entidad Dueña del efecto (controlador original de la carta)
    /// </summary>
    public EntityController owner;

    /// <summary>
    /// Entidad que tiene el control del efecto/carta actualmente
    /// </summary>
    public EntityController controller;


    /// <summary>
    /// Create context without owner or controller
    /// </summary>
    /// <param name="self">Object in question</param>
    public TargettingContext(GameObject self){
        this.self = source = self; 
    }
}