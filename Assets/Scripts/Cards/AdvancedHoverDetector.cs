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

        void Start()
        {
            thisCard = GetComponent<Card>();
        }


        protected override void OnMouseEnter()
        {
            if(thisCard.isFaceUp()){
                base.OnMouseEnter();
            }
        }

    
    }
}
