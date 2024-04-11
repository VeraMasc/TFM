using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Effect
{
    /// <summary>
    /// Gestiona los targets que requieren input del jugador
    /// </summary>
    [Serializable]
    public abstract class ManualTargeter:EffectTargeter, IManual
    {
        //TODO: manual targeting

        /// <summary>
        /// Obtiene la pool base de objetos "targeteables"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual IEnumerable<ITargetable> targetPool(Effect.Context context){
            //Por defecto devolver todos los elementos en el campo
            return GameController.singleton.getTargetablesOnBoard();
        }

        [SerializeField]
        private bool _chooseOnCast;

        bool IManual.chooseOnCast => _chooseOnCast;

        public virtual IEnumerator awaitUserInput(Effect.Context context){
            yield return null;
        }
        public override void resolveTarget(Effect.Context context){
            
        }

    }

    /// <summary>
    /// Interfaz que engloba todos las partes de los efectos que requieren inputs manuales
    /// </summary>
    public interface IManual {
        /// <summary>
        /// Espera a que se produzcan los inputs necesarios para que el target obtenga su valor
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerator awaitUserInput(Effect.Context context);

        /// <summary>
        /// Indica si la decisión se realiza en el momento que la carta entra en la pila de resolución
        /// </summary>
        public bool chooseOnCast{get;}
    }
}

