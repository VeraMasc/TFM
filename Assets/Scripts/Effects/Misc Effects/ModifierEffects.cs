using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace Effect{
    /// <summary>
    /// Efecto que añade un modificador
    /// </summary>
    [Serializable]
    public class AddModifier : Targeted, IValueEffect
    {


        [SerializeReference,SubclassSelector]
        public IValue param;

        /// <summary>
        /// Lista de modificadores a añadir
        /// </summary>
        [SerializeReference,SubclassSelector]
        public List<BaseModifier> modifiers;


        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            var val = param?.getValueObj(context);
            if(target is Card card){
                foreach(var mod in modifiers){
                    CardModifiers.addModifier(card,mod,val);
                }
                if(card.data is MyCardSetup setup){
                    var zone = card.Group?.GetComponent<GroupZone>();
                    setup.effects.refreshAbilitySuscriptions(zone.zone);
                }
            }
            else{
                Debug.LogError("Modifiers can only be added to cards");
            }
            yield break;
        }

    }


    public class RemoveModifier : Targeted, IValueEffect
    {

        
        /// <summary>
        /// Lista de modificadores a eliminar
        /// </summary>
        [SerializeReference,SubclassSelector]
        public List<BaseModifier> modifiers;


        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Card card){
                foreach(var mod in modifiers){
                    //TODO: implement
                    CardModifiers.addModifier(card,mod);
                }
                
            }
            else{
                Debug.LogError("Modifiers can only be added to cards");
            }
            yield break;
        }

    }

    [Serializable]
    public class GetModifierValues : Targeted, IValueEffect
    {
        public List<string> ids;


        
        public override IEnumerator executeForeach(ITargetable target,CardResolveOperator stack, Context context)
        {
            if(target is Card card){
                var modifiers = CardModifiers.getModifiers(card)
                    .OfType<ValueModifier>();

                var mod = modifiers.Where( m => ids.Contains(m.id) ).FirstOrDefault();
                    
                if(mod != null){
                    context.previousValues.Add(mod.value);
                    Debug.Log(mod.value);
                }else
                    Debug.LogError("Modifier Value Not FOund");
            }
            else{
                Debug.LogError("Modifiers can only be extracted from cards");
            }
            yield break;
        }
    }
}
