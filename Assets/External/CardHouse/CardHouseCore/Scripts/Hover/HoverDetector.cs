using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardHouse
{
    public class HoverDetector : Toggleable
    {
        public UnityEvent OnHover;
        public UnityEvent OnUnHover;

        protected virtual void OnMouseEnter()
        {
            if (!IsActive || (EventSystem.current?.IsPointerOverGameObject() ?? false))
                return;

            OnHover.Invoke();
        }

        protected virtual void OnMouseExit()
        {
            if (!IsActive)
                return;

            OnUnHover.Invoke();
        }
    }
}
