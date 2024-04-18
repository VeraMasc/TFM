using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardHouse
{
    public class ClickDetector : Toggleable
    {
        public UnityEvent OnPress;
        public UnityEvent OnButtonClicked;

        public GateCollection<NoParams> ClickGates;

        void OnMouseDown()
        {
            if (IsActive && ClickGates.AllUnlocked(null) && !EventSystem.current.IsPointerOverGameObject())
            {
                OnPress.Invoke();
            }
        }

        void OnMouseUpAsButton()
        {
            if (IsActive && ClickGates.AllUnlocked(null))
            {
                OnButtonClicked.Invoke();
            }
        }
    }
}
