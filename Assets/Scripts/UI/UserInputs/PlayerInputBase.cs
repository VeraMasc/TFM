using System.Collections;
using Common.Coroutines;
using Effect;
using UnityEngine;

/// <summary>
/// Base de todas los elementos de interfaz que requieren algún tipo de input del jugador
/// </summary>
public abstract class PlayerInputBase : MonoBehaviour
{
    /// <summary>
    /// Indica si el proceso de introducir inputs ha terminado
    /// </summary>
    public bool isFinished;

    /// <summary>
    /// Indica que no se ha introducido un valor sino que se ha cancelado la obtención de input
    /// </summary>
    public bool isCancelled;

    /// <summary>
    /// Corrutina que espera a que se termine de introducir los inputs
    /// </summary>
    public IEnumerator waitTillFinished => UCoroutine.YieldAwait(()=> this.isFinished);

    /// <summary>
    /// Valor introducido por el jugador
    /// </summary>
    public object inputValue;

    /// <summary>
    /// Acción de confirmar la decisión tomada. Se activa al pulsar el botón de confirmar
    /// </summary>
    public virtual void confirmChoice(){
        isFinished=true;
    }

    public virtual void cancelChoice(){
        isCancelled=true;
        isFinished=true;
    }

    public virtual void setInputConfig(InputParameters parameters){

    }

}

/// <summary>
/// Gestiona los posibles parámetros que se pueden enviar a un input
/// </summary>
public class InputParameters
{
    /// <summary>
    /// Testo a mostrar al jugador
    /// </summary>
    public string text;
    /// <summary>
    /// Contexto en el que se llama al input
    /// </summary>
    public Effect.Context context;

    /// <summary>
    /// Lista de valores que ha de usar el input
    /// </summary>
    public object[] values;
    /// <summary>
    /// Objeto con otros valores de configuración
    /// </summary>
    public object extraConfig;

    /// <summary>
    /// Indica que la acción se puede cancelar
    /// </summary>
    public bool canCancel = true;
}