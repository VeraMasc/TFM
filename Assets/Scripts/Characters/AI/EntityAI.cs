using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomInspector;
using UnityEngine;

/// <summary>
/// Clase base de la IA que controla entidades
/// </summary>
public class EntityAI : MonoBehaviour
{

    /// <summary>
    /// Entidad a controlar
    /// </summary>
    [SelfFill]
    public Entity entity;

    /// <summary>
    /// Comprueba entre todas las acciones disponibles cual es la mejor
    /// </summary>
    public virtual IActionable findBestAction(IEnumerable<IActionable> options){

        //Selección aleatoria
        return options.ElementAt(Random.Range(0, options.Count()));
    }

    /// <summary>
    /// Ejecuta el turno de la IA
    /// </summary>
    public virtual IEnumerator doTurn(){
        Debug.Log("Doing Turn");
        yield break;
    }

    /// <summary>
    /// Botón de test que fuerza el inicio del turno
    /// </summary>
    [NaughtyAttributes.Button(enabledMode: NaughtyAttributes.EButtonEnableMode.Playmode)]
    protected void startTurn(){
        StartCoroutine(doTurn());
    }

    /// <summary>
    /// Ejecuta la reacción de la IA
    /// </summary>
    public virtual IEnumerator doReaction(){
        yield break;
    }


}

/// <summary>
/// Interfaz de todas las cosas que una IA puede escoger hacer
/// </summary>
public interface IActionable{

}
