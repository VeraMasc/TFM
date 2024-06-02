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
using Effect.Condition;

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
        [Serializable]
        public abstract class ModifierPreset{

            /// <summary>
            /// Aplica el modificador a la carta
            /// </summary>
            /// <param name="card"></param>
            public virtual void applyTo(Card card, Context context){

            }

            /// <summary>
            /// Aplica el preset en forma de habilidad SI PUEDE!!!
            /// </summary>
            public virtual void applyAsAbility(Card card, Context context){

            }
        }


        /// <summary>
        /// Overdo: Mi versión  de flashback
        /// </summary>
        [Serializable]
        public class OverDo:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();

            protected Ability getAbility(ActionCard action,Context context){

                BaseModifier exileMod =new ForceZoneModifier(){    
                        sendTo = GroupName.Exile,
                        singleUse = true,

                    };
                var mana = (List<Mana>)cost?.getValueObj(context);
                return new ZoneCastAbility(){
                                id = "overdo",
                                cost = new ManaCost(mana),
                                speed = action.speedType,
                                activeZoneList = new List<GroupName>(){GroupName.Discard},
                                //Fuerza al exilio tras usarlo
                                effects = new List<EffectScript>{
                                    new AddModifier(){
                                        targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                                        modifiers = new(){
                                            exileMod
                                        }
                                    }
                                }
                            };
            }
            public override void applyTo(Card card, Context context){
                if( card.data is ActionCard action){

                    Debug.Log($"Apply to {card}");

                    var zoneCast =  getAbility(action, context);

                    //Crea el modificador de cast
                    var castmodifier = new AbilityModifier(){
                        abilities = new(){
                           zoneCast
                        }
                    };
                    
                    
                    CardModifiers.addModifier(card,castmodifier);
                }
            }

            public override void applyAsAbility(Card card, Context context)
            {
                if( card.data is ActionCard action){

                    Debug.Log($"Apply to {card}");

                    var zoneCast =  getAbility(action, context);
                    action.effects.abilities.Add(zoneCast);
                }
            }
        }

        
        /// <summary>
        /// Rethink: Mi versión  de cycling
        /// </summary>
        [Serializable]
        public class Rethink:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();

            protected Ability getAbility(ActionCard action,Context context){

                var mana = (List<Mana>)cost?.getValueObj(context);
                return new ActivatedZoneAbility(){
                                id = "rethink",
                                cost = new ManaCost(mana),
                                speed = SpeedTypes.Reaction,
                                activeZoneList = new List<GroupName>(){GroupName.Hand},
                                //Fuerza al exilio tras usarlo
                                effects = new List<EffectScript>{
                                    new ReDraw(){
                                        targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                                    }
                                }
                            };
            }
            public override void applyTo(Card card, Context context){
                if( card.data is ActionCard action){

                    var ability =  getAbility(action, context);

                    //Crea el modificador de cast
                    var castmodifier = new AbilityModifier(){
                        abilities = new(){
                           ability
                        }
                    };
                    
                    
                    CardModifiers.addModifier(card,castmodifier);
                }
            }

            public override void applyAsAbility(Card card, Context context)
            {
                if( card.data is ActionCard action){

                    Debug.Log($"Apply to {card}");

                    var zoneCast =  getAbility(action, context);
                    action.effects.abilities.Add(zoneCast);
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

        [Serializable]
        public class DurationCountdown:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue duration = new Numeric(3);

            public override void applyAsAbility(Card card, Context context)
            {
                if( card.data is ActionCard action){

                    var num = (int)duration.getValueObj(context) ;

                    var ETB = new HiddenTriggeredAbility(){
                        effects = new(){
                            new SetCounters(){
                                mode=new SetCounters.Set(){
                                    amount = new Numeric(num),
                                    counterType = new TextString("Duration"),
                                },
                                targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                            }
                        },
                        source = card,
                        activeZoneList= new (){ GroupName.None},
                        trigger = GameController.singleton.triggerManager.onEnter,
                    };

                    var countdown= new ImplicitTriggeredAbility(){
                        condition= new TurnCondition(){
                            turnOwner = new ContextualEntityTargeter(){
                                contextual = ContextualEntityTargets.controller
                            },
                        },
                        activeZoneList= new(){ GroupName.Board},
                        id="Duration",
                        text ="At the beginning of each of your turns, remove a Duration counter. Then destroy it if it has no Duration Counters",
                        trigger = GameController.singleton.triggerManager.onBeginTurn,
                        //TODO: FInish
                        effects = new(){
                            new SetCounters(){
                                mode= new SetCounters.Add(){
                                    amount = new Numeric(-1),
                                    counterType=  new TextString("Duration")
                                },
                                targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                            },
                            new ExecuteIf(){
                                conditions= new(){new CounterCondition(){
                                        counterType = "Duration",
                                        innerCondition = new AmountCondition(){
                                            min=0,
                                            max=0,
                                        }
                                    }
                                },
                                input = new ContextualObjectTargeter(ContextualObjTargets.self),
                                effects = new(){
                                    new Destroy(){
                                        targeter = new ContextualObjectTargeter(ContextualObjTargets.self)
                                    }
                                }
                            }
                        },
                        source = card

                    };
                    action.effects.abilities.Add(ETB);
                    action.effects.abilities.Add(countdown);
                }
            }
            
        }
    }
    
}