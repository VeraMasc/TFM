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
    public bool infiniteMana;

    /// <summary>
    /// No requiere que tengas el maná necesario para lanzar hechizos (se sigue gastando).
    /// Solo para debug
    /// </summary>
    public bool ignoreRequirements;
    public TextMeshPro display;

    /// <summary>
    /// Lista de los "pips" de maná individuales
    /// </summary>
    public List<Mana> pips = new();


    public bool tryToPay(ManaCost cost){
        if(canPay(cost)){
            pay(cost);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Comprueba si es posible pagar el coste
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public bool canPay(ManaCost cost){
        Debug.Log("Mana cost checked");

        if(ignoreRequirements)
            return true;

        //Check total value
        if(pips.Count < cost.value)
            return false;
        
        //Check colored
        var colored = cost.coloredCost;
        var unused = pips.ToList();
        foreach(var pip in colored){
            var str = pip.ToString();
            var index = unused.FindIndex(p=> p.color.ToString() == str);
            if(index==-1) //No match
                return false;
            //Remove used
            unused.RemoveAt(index);
        }
        
        return true;
    }

    public void pay(ManaCost cost){
        //TODO: implementar pago
        if(infiniteMana)
            goto refresh;
        
        //pay colored
        var colored = cost.coloredCost;
        foreach(var pip in colored){
            var str = pip.ToString();
            var index = pips.FindIndex(p=> p.color.ToString() == str);
            if(index!=-1)
                pips.RemoveAt(index); //Remove spent
        }

        //Pay generic
        var amount = cost.value - colored.Length;
        if(amount<=0)
            goto refresh; //No hay más que pagar
        var colorless = pips.Where(p => p.color == Mana.Colors.C)
            .Take(amount).ToArray();
        pips = pips.Except(colorless).ToList(); //Remove spent colorless
        amount-=colorless.Length;

        if(amount<=0)
            goto refresh; //No hay más que pagar
        pips.RemoveRange(0,amount); //Remove colored

    refresh:
        updateDisplay();
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
        pips = pips.OrderBy(m=>m.color).ToList();

        var generic = pips.Count( m => m.color == Mana.Colors.C);

        var letters = pips.Where(m => m.color != Mana.Colors.C)
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