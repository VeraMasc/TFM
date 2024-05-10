using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CardHouse
{
    /// <summary>
    /// Detector de hover avanzado (Ignora el hover en ciertos casos)
    /// </summary>
    public class AdvancedHoverDetector :HoverDetector 
    {
        private Card thisCard;
        private DragDetector dragDetector;

        void Start()
        {
            thisCard = GetComponent<Card>();
            dragDetector = GetComponent<DragDetector>();
        }


        protected override void OnMouseEnter()
        {
            if(thisCard.isFaceUp() && dragDetector?.isDragging != true){
                base.OnMouseEnter();
            }
        }

    
    }
}
