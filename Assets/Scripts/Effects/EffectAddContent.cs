using System;
using System.Collections;
using CardHouse;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class AddContent : EffectScript
    {
        /// <summary>
        /// Quién roba las cartas
        /// </summary>
        [SerializeReference, SubclassSelector]
        EffectTargeter target; 
        public int amount = 2;

        public override IEnumerator execute(Card self)
        {
            var hasParent = self.Group.GetComponent<Card>() != null;
            var op =ExplorationController.singleton.content.GetComponent<CardTransferOperator>();
            op.Transition.Destination = self.Group;
            op.Activate();
            yield return op.currentAction;
        }
    }
}
