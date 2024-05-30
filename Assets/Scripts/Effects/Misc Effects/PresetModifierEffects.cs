using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using CustomInspector;
using Effect.Value;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using Effect.Preset;

namespace Effect{
    /// <summary>
    /// Efecto que añade un modificador usando un preset específico
    /// </summary>
    [Serializable]
    public class AddPresetModifier : Targeted, IValueEffect
    {
        [SerializeReference,SubclassSelector]
        public ModifierPreset preset;

        public override IEnumerator executeForeach(ITargetable target, CardResolveOperator stack, Context context)
        {
            if(target is Card card){
                preset.applyTo(card,context);
                if(card.data is ActionCard action){
                    var zone = card.Group.GetComponent<GroupZone>();
                    action.effects.refreshAbilitySuscriptions(zone?.zone ?? GroupName.None);
                }
            }
            yield break;
        }
    }
    
    namespace Preset
    {
        /// <summary>
        /// Base de todos los presets de modificadores
        /// </summary>
        public abstract class ModifierPreset{

            /// <summary>
            /// Aplica el modificador a la carta
            /// </summary>
            /// <param name="card"></param>
            public virtual void applyTo(Card card, Context context){

            }
        }


        /// <summary>
        /// Overdo: Mi versión  de flashback
        /// </summary>
        [Serializable]
        public class OverDo:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();

            public override void applyTo(Card card, Context context){
                if( card.data is ActionCard action){

                    BaseModifier exileMod =new ForceZoneModifier(){    
                        sendTo = GroupName.Exile,
                        singleUse = true,

                    };
                    var mana = (List<Mana>)cost?.getValueObj(context);

                    Debug.Log($"Apply to {card}");

                    var zoneCast =  new ZoneCastAbility(){
                                cost = new ManaCost(mana),
                                activeZoneList = new List<GroupName>(){GroupName.Discard},
                                //Fuerza al exilio tras usarlo
                                effects = new List<EffectScript>{
                                    new AddModifier(){
                                        modifiers = new(){
                                            exileMod
                                        }
                                    }
                                }
                            };

                    zoneCast.speed = action.speedType;
                    //Crea el modificador de cast
                    var castmodifier = new AbilityModifier(){
                        abilities = new(){
                           zoneCast
                        }
                    };
                    
                    
                    CardModifiers.addModifier(card,castmodifier);
                }
            }
        }

        [Serializable]
        public class Prophecise:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();

            public override void applyTo(Card card,Context context){

            }
        }
    }
    
}