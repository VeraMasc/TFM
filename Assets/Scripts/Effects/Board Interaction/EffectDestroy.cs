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
                    if(card.attachedGroup != null){
                        //TODO: Destroygroup default pile
                        card.attachedGroup.destroyGroup(GroupName.Discard);    
                    }
                }
                else if (target is CardGroup group){ //If group use the group
                    group.destroyGroup(GroupName.Discard);
                }
                GameObject.Destroy((target as Component).gameObject);
                //TODO: add Destruction animation
                yield return new WaitForSeconds(0.2f);
                var source = ExplorationController.singleton.content;
               
                
                
            }
        }
    }
}
