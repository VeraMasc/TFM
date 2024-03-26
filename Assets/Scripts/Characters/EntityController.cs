using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;

/// <summary>
/// Cualquier entidad que usa cartas de acción
/// </summary>
public class EntityController : MonoBehaviour
{
    /// <summary>
    /// Mano con cartas de la entidad
    /// </summary>
    public CardGroup hand;

    public CardGroup deck;


    /// <summary>
    /// Hace que el jugador robe cartas
    /// </summary>
    /// <param name="amount"> cuantas cartas ha de robar</param>
    public void draw(int amount){
        //TODO: Card transfer
    }
}
