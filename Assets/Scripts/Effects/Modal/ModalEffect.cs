using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardHouse;
using Common.Coroutines;
using DTT.Utils.Extensions;
using Effect;
using Effect.Status;
using UnityEngine;


namespace Effect
{


    [Serializable]
    /// <summary>
    /// Permite seleccionar entre varios modos
    /// </summary>
    public class ModalEffect : EffectScript, IValueEffect, IPrecalculable
    {
        public int maxChoices=1;
        /// <summary>
        /// Lista de modos posibles
        /// </summary>
        public List<CardMode> modes;

        /// <summary>
        /// Modos escogidos
        /// </summary>
        [NonSerialized]
        public List<int> chosen=new();
        public IEnumerator precalculate(CardResolveOperator stack, Context context)
        {
            var modeSettings = modes.Select(m => new ModalOptionSettings(){tag=m.id}).ToArray();
            yield return UCoroutine.Yield(GameUI.singleton.getInput(GameUI.singleton.prefabs.cardSelectInput, 
                obj => {chosen = (List<int>)obj;},
                new InputParameters{ values= (object[])modeSettings, context=context,
                    extraConfig = new CardSelectInput.ExtraInputOptions(){maxChoices=maxChoices}
                }));

            //Precalculate chosen modes
            if(chosen?.Any()??false){
                int index=0;
                foreach(var mode in modes){
                    if(chosen.Contains(index)){
                        yield return UCoroutine.Yield(Precalculate.precalculateEffects(mode.effects,context));

                    }
                    index++;
                }

                //Change text temporarily
                if(context.self is Card card && card.data is MyCardSetup setup){
                    var links = setup.getTextLinks(chosen.Select(index=> modes[index].id));

                    setup.tempText = String.Join("\n", links.Select(l => l.getRawLinkText()));
                    setup.applyText();
                }
            }
            
        }

        public override IEnumerator execute(CardResolveOperator stack, Context context)
        {
            int index=0;
            foreach(var mode in modes){
                if(chosen.Contains(index)){
                    foreach(var effect in mode.effects){
                        yield return UCoroutine.Yield(effect.execute(stack,context));
                    }
                }
                index++;
            }
            
        }


        public static IEnumerator castModal(Card card, IEnumerable<ModalOptionSettings> modes, Entity caster = null){

            var modeSettings = modes.ToArray();
            List<int> ret = null;
            if(card?.data is MyCardSetup setup){
                //Generar diálogo modal
                yield return UCoroutine.Yield(GameUI.singleton.getInput(GameUI.singleton.prefabs.cardSelectInput, 
                obj => {
                    ret = (List<int>)obj;
                },
                new InputParameters{ values= (object[])modeSettings, 
                    context= new Context(setup?.effects?.context)
                }));
                
                //Quitar outline al acabar
                card.outlineRenderer?.gameObject?.SetActive(false);

                //Usar resultado
                if(ret != null && ret.Count >0){
                    var index = ret.First();
                    Debug.Log($"Cast modal index: {index}");
                    var settings = modeSettings[index];
                    

                    if(settings.tag == string.Empty && settings.ability == null){//Default cast
                        yield return UCoroutine.Yield(CardResolveOperator.singleton.castCard(card));
                    }
                    else if (settings.ability is ActivatedAbility activated){
                        var ownership = card.GetComponent<CardOwnership>();
                        caster ??= ownership?.controller ?? card.Group?.GetComponent<GroupZone>()?.owner;
                        yield return UCoroutine.Yield(activated.activateAbility(caster));
                    }
                }
            }
            
        }
    }

    [Serializable]
    public class CardMode{
        public string id;
        [SerializeReference, SubclassSelector]
        public List<EffectScript> effects;
        
    }
}