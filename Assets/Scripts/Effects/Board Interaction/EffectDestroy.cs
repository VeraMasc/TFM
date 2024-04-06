using System;
using System.Collections;
using CardHouse;
using UnityEngine;

namespace Effect{
    /// <summary>
    /// Efecto que añade contenido a una habitación
    /// </summary>
    [Serializable]
    public class Destroy : Targeted
    {
    

        public override IEnumerator execute(CardResolveOperator stack, TargettingContext context)
        {
            var targets = targeter.getTargets(context);
           
            foreach(var target in targets){
                
                

                if(target is Card card){ //If card use attach group
                    
                    card.DestroyCard();
                }
                else if (target is CardGroup group){ //If group use the group
                    group.destroyGroup(GroupName.Discard);
                }
                //TODO: add Destruction animation
                yield return new WaitForSeconds(0.2f);
                var source = ExplorationController.singleton.content;
               
                
                
            }
        }
    }
}
