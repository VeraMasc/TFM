using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardHouse
{
    public class HoverDetector : Toggleable
    {
        public UnityEvent OnHover;
        public UnityEvent OnUnHover;

        void OnMouseEnter()
        {
            if (!IsActive || EventSystem.current.IsPointerOverGameObject())
                return;

            OnHover.Invoke();
        }

        void OnMouseExit()
        {
            if (!IsActive)
                return;

            OnUnHover.Invoke();
        }
    }
}
