using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Effect;
using TMPro;
using UnityEngine;



/// <summary>
/// Gestiona la la representación y gestión del "maná"
/// </summary>
public class ManaPool : MonoBehaviour
{
    /// <summary>
    /// Hace que el maná no se gaste. Solo para debug
    /// </summary>
    bool infiniteMana;
    public TextMeshPro display;

    public List<Mana> mana = new();


    public bool tryToPay(ManaCost cost){
        if(canPay(cost)){
            pay(cost);
            return true;
        }
        return false;
    }
    public bool canPay(ManaCost cost){
        //TODO: implementar comprobación
        Debug.Log("Mana cost checked");
        return true;
    }

    public void pay(ManaCost cost){
        //TODO: implementar pago
        
    }

    public void loseAll(){
        //TODO: implementar perder todo el maná.
    }


    /// <summary>
    /// Refresca la representación del maná
    /// </summary>
    [NaughtyAttributes.Button]
    public void updateDisplay(){
        //Sort mana
        mana = mana.OrderBy(m=>m.color).ToList();

        var generic = mana.Count( m => m.color == Mana.Colors.C);

        var letters = mana.Where(m => m.color != Mana.Colors.C)
            .Select(m => $"{{{m.color.ToString()}}}");
        
        var rawText = String.Join("",letters);
        if(generic>0){
            rawText+= $"{{{generic}}}";
        }
        display.text = MyCardDefinition.parseCost(rawText);

    }
}


/// <summary>
/// Clase que representa los tipos de maná
/// </summary>
[Serializable]
public class Mana{
    /// <summary>
    /// Color del maná
    /// </summary>
    public Colors color;


    public Mana(){

    }

    public Mana(Colors color){
        this.color = color;
    }


    /// <summary>
    /// Enum de los colores de maná que hay
    /// </summary>
    public enum Colors{
        /// <summary>
        /// Colorless
        /// </summary>
        C,
        /// <summary>
        /// Martial
        /// </summary>
        M,
        /// <summary>
        /// Dark
        /// </summary>
        D,
        /// <summary>
        /// Vital
        /// </summary>
        V,
        /// <summary>
        /// Foresight
        /// </summary>
        F,
        /// <summary>
        /// Wit
        /// </summary>
        W,
        /// <summary>
        /// Natural
        /// </summary>
        N,
        /// <summary>
        /// Aberrant
        /// </summary>
        A,
    }
}