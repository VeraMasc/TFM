using CustomInspector;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace CardHouse
{
    public class DragDetector : Toggleable
    {
        public GateCollection<NoParams> DragGates;
        public UnityEvent OnDragStart;

        [FormerlySerializedAs("DropGates")]
        public GateCollection<DropParams> GroupDropGates;
        public GateCollection<TargetCardParams> TargetCardGates;
        public UnityEvent OnDragEnd;

        /// <summary>
        /// Indicates if the drag is currently active
        /// </summary>
        [ReadOnly]
        public bool isDragging;

        void OnMouseDown()
        {
            if (!IsActive || !DragGates.AllUnlocked(null))
                return;
            isDragging = true;
            OnDragStart.Invoke();
        }

        void OnMouseUp()
        {
            if (!IsActive || !DragGates.AllUnlocked(null))
                return;
            isDragging = false;
            OnDragEnd.Invoke();
        }
    }
}
