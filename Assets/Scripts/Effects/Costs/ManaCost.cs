using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CustomInspector;
using UnityEngine;

namespace Effect{
    [Serializable]
    public class ManaCost:ICost
    {
        /// <summary>
        /// Coste en formato de texto
        /// </summary>
        [Validate("isValid", errorType = MessageBoxType.Warning)]
        [Hook(nameof(parseCost))]
        [LabelSettings(LabelStyle.NoLabel)]
        public string costText;

        /// <summary>
        /// Valor numérico total del coste
        /// </summary>
        [ReadOnly, HorizontalGroup(true),Indent]
        public int value;
        /// <summary>
        /// Colores que hay en el coste
        /// </summary>
        [ReadOnly, HorizontalGroup(), Indent]
        public string colors;


        public ManaCost(){

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="original"></param>
        public ManaCost(ManaCost original){
            costText = original.costText;
            value = original.value;
            colors = original.colors;
        }

        public ManaCost(IEnumerable<Mana> pips){
            var colorless = pips.Where(p=>p.color== Mana.Colors.C).Count();
            var colored = string.Join("",pips.Where(p=>p.color != Mana.Colors.C));
            costText = colorless>0? colorless +colored: colored;
            parseCost();
        }

        [NonSerialized]
        public List<Mana> pips = new();

        /// <summary>
        /// Devuelve el valor en el formato de texto de las cartas
        /// </summary>
        /// <returns></returns>
        public string asCardText(){
            var segments = manaSegments.Match(costText);
            var digits = segments.Groups[1];
            var colored = segments.Groups[2];
            var ret = "";

            if(digits.Length>0){
                ret+=$"{{{digits}}}";
                
            }
            if(colored.Length>0){
                foreach( var pip in colored.Value){
                    ret+=$"{{{pip}}}";
                }
            }
            return ret;
        }

        /// <summary>
        /// Comprueba que el coste tiene sentido
        /// </summary>
        /// <returns></returns>
        public bool isValid(){
            return manaExpression?.IsMatch(costText) ?? false;
        }

        /// <summary>
        /// Expresión regular que describe un coste de maná válido
        /// </summary>
        protected static Regex manaExpression = new(@"^X*\d*[MDVFWNA]*[+]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);


        /// <summary>
        /// Expresión regular que separa cada parte del coste
        /// </summary>
        protected static Regex manaSegments = new(@"^X*(\d*)([MDVFWNA]*)([+]?)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void parseCost(){
            var segments = manaSegments.Match(costText);
            if(segments == null)
                return;
            var digits = segments.Groups[1];
            value = int.TryParse(digits.Value, out value)? value: 0;
            pips.AddRange(Enumerable.Repeat(new Mana(Mana.Colors.C),digits.Length));

            //Extrae el coste con "colores"
            var coloredGroup = segments.Groups[2];
            value += coloredGroup.Length;
            colors = new string(coloredGroup.Value.ToUpper().Distinct().ToArray());
            //To pips
            foreach(var letter in coloredGroup.Value){
                var mana = (Mana.Colors)Enum.Parse(typeof(Mana.Colors), letter.ToString());
                pips.Add(new Mana(mana));
            }
        }


        /// <summary>
        /// Indica si el coste puede ser pagado en el contexto actual
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool canBePaid(Effect.Context context){
            if(context?.controller == null) //No hay nadie que lo puede pagar
                return false;
                
            return context.controller.mana.canPay(this);
        }

        public IEnumerator payCost(Context context){
            if(context?.controller == null) //No hay nadie que lo puede pagar
                 yield break;
            context.controller.mana.pay(this);
            yield break;
        }

        /// <summary>
        /// Parte del coste "coloreada"
        /// </summary>
        public string coloredCost {
            get {
                var segments = manaSegments.Match(costText);
                var coloredGroup = segments.Groups[2];
                return coloredGroup.Value.ToUpper();
            }
        }

        /// <summary>
        /// Resta un coste a otro
        /// </summary>
        /// <param name="cost">Cantidad a restar</param>
        /// <returns></returns>
        public ManaCost subtract(ManaCost cost){
            var ret = new ManaCost(cost);
            return ret;
        }
    }


    /// <summary>
    /// Versión simplificada del coste de maná
    /// </summary>
    [Serializable]
    public class SimpleManaCost{
        public string costText;
    }

}