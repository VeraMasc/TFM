using System;
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

        /// <summary>
        /// Comprueba que el coste tiene sentido
        /// </summary>
        /// <returns></returns>
        public bool isValid(){
            return manaExpression.IsMatch(costText);
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
            var digits = segments.Groups[1];
            value = int.TryParse(digits.Value, out value)? value: 0;
            //Extrae el coste con "colores"
            var colored = segments.Groups[2];
            value += colored.Length;
            colors = new string(colored.Value.ToUpper().Distinct().ToArray());
        }

        /// <summary>
        /// Checks if the cost can be paid
        /// </summary>
        public bool canBePaid(Effect.Context context){
            //TODO: Implement mana costs
            throw new NotImplementedException();
        }
    }

}