using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardHouse
{
    public class ClickDetector : Toggleable
    {
        public UnityEvent OnPress;
        public UnityEvent OnButtonClicked;

        /// <summary>
        /// Teclas que actuan como si fueran un click en este botón
        /// </summary>
        public List<KeyCode> keyBindings;

        public GateCollection<NoParams> ClickGates;

        void OnMouseDown()
        {
            if (!(EventSystem.current?.IsPointerOverGameObject()?? false))
            {
                executePress();
            }
        }

        /// <summary>
        /// Ejecuta los efectos de hacer click al botón (haya click o no)
        /// </summary>
        public void executePress(){
            if (IsActive && ClickGates.AllUnlocked(null))
            {
                OnPress.Invoke();
            }
        }

        /// <summary>
        /// Ejecuta los efectos de dejar de hacer click al botón (haya click o no)
        /// </summary>
        public void executeRelease(){
            if (IsActive && ClickGates.AllUnlocked(null))
            {
                OnButtonClicked.Invoke();
            }
        }

        void OnMouseUpAsButton()
        {
            executeRelease();
        }

        void Update()
        {
            foreach(var key in keyBindings){
                if(Input.GetKeyDown(key)){
                    executePress();
                }else if(Input.GetKeyUp(key)){
                    executeRelease();
                }
            }
        }
    }
}
