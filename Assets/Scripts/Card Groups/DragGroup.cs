using System.Collections;
using System.Collections.Generic;
using CardHouse;
using UnityEngine;
using CustomInspector;
using UnityEngine.Events;

/// <summary>
/// Hace que el grupo pueda ser movido dinámicamente junto con sus cartas
/// </summary>
public class DragGroup : MonoBehaviour
{
    private Homing seeker;

    private DragDetector detector;

    /// <summary>
    /// Grupo de cartas al que afecta
    /// </summary>
    [SelfFill]
    public CardGroup group;


    /// <summary>
    /// Collider que detecta las interacciones con el mazo
    /// </summary>
    [SelfFill]
    public Collider2D groupCollider;

    /// <summary>
    /// Collider secundario del grupo
    /// </summary>
    [ForceFill]
    public Collider2D secondaryCollider;
    /// <summary>
    /// Registra si el grupo está en movimiento
    /// </summary>
    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        seeker =transform.parent?.GetComponent<Homing>();
        detector =transform.parent?.GetComponent<DragDetector>();

        //Añadir listeners
        detector.OnDragStart.AddListener(onChangeState);
        detector.OnDragEnd.AddListener(onChangeState);
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = seeker?.seeking ?? false;
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
        

        //Desactivar el collider mientras se mueven las cartas
        groupCollider.enabled= !detector.isDragging;
        secondaryCollider.enabled= !detector.isDragging;
    }
}
