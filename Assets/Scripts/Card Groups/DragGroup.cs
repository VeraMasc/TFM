using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using CustomInspector;

/// <summary>
/// Hace que el grupo pueda ser movido dinámicamente junto con sus cartas
/// </summary>
public class DragGroup : MonoBehaviour
{
    private Homing seeker;

    /// <summary>
    /// Grupo de cartas al que afecta
    /// </summary>
    [SelfFill]
    public CardGroup group;
    /// <summary>
    /// Registra si el grupo está en movimiento
    /// </summary>
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        seeker =transform.parent?.GetComponent<Homing>();
    }

    // Update is called once per frame
    void Update()
    {

        var newMoving = seeker?.seeking ?? false;
        if (newMoving != isMoving){
            onChangeState(); //Desactivar grupo mientras se mueve
        }
        isMoving = newMoving;
    }

    void LateUpdate()
    {
        //Mover cartas con el grupo
        if(isMoving){
            group.ApplyStrategy();
        }
    }

    /// <summary>
    /// Gestiona el paso de estar parado a moverse y viceversa
    /// </summary>
    public void onChangeState(){

    }
}
