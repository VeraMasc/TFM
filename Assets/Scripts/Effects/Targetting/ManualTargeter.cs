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
    public class BaseManualTargeter:EffectTargeter, IManual
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


        public virtual IEnumerator awaitUserInput(Effect.Context context){
            yield return null;
        }
        public override void resolveTarget(Effect.Context context){
            
        }

    }


    [Serializable]
    public class SingleTypeManualTargeter:BaseManualTargeter{
        
    }

    /// <summary>
    /// Interfaz que engloba todos las partes de los efectos que requieren inputs manuales //TODO: Rework IManual or remove
    /// </summary>
    public interface IManual {
        /// <summary>
        /// Espera a que se produzcan los inputs necesarios para que el target obtenga su valor
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerator awaitUserInput(Effect.Context context);

        
    }
}

