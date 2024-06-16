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
                if( card.data is ActionCard action){

                    var ability =  getAbility(action, context);

                    //Crea el modificador
                    var modifier = new AbilityModifier(){
                        abilities = new(){
                           ability
                        }
                    };
                    
                    
                    CardModifiers.addModifier(card,modifier);
                }
            }

            /// <summary>
            /// Aplica el preset en forma de habilidad SI PUEDE!!!
            /// </summary>
            public virtual void applyAsAbility(Card card, Context context){
                if( card.data is ActionCard action){
                    var ability =  getAbility(action, context);
                    if(ability != null)
                        action.effects.abilities.Add(ability);
                }
            }

            public virtual Ability getAbility(MyCardSetup setup,Context context){
                return null;
            }

        }


        /// <summary>
        /// Overdo: Mi versión  de flashback
        /// </summary>
        [Serializable]
        public class OverDo:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();

            public override Ability getAbility(MyCardSetup setup,Context context){

                BaseModifier exileMod =new ForceZoneModifier(){    
                        sendTo = GroupName.Exile,
                        singleUse = true,

                    };
                var mana = (List<Mana>)cost?.getValueObj(context);
                var speed = setup is ActionCard action? action.speedType: SpeedTypes.Action;

                return new ZoneCastAbility(){
                                id = "overdo",
                                cost = new ManaCost(mana),
                                speed = speed,
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

        }

        
        /// <summary>
        /// Rethink: Mi versión  de cycling
        /// </summary>
        [Serializable]
        public class Rethink:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();

            public override  Ability getAbility(MyCardSetup setup,Context context){

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
            

        }

        [Serializable]
        public class Prophesy:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue cost = new ManaValue();


            public override Ability getAbility(MyCardSetup setup,Context context){
                var speed = setup is ActionCard action? action.speedType: SpeedTypes.Action;
                var enabler = new HiddenTriggeredAbility(){
                    id = "removeSelf",
                    activeZoneList= new List<GroupName>(){GroupName.Lost},
                    condition = new Effect.Condition.TurnCondition(){
                        turnOwner = new ContextualEntityTargeter(ContextualEntityTargets.controller),
                    },
                    trigger = TriggerManager.instance.beforeBeginTurn,
                    effects = new List<EffectScript>{
                        new Effect.AddModifier(){
                            targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                            modifiers = new List<BaseModifier>{
                                new AbilityModifier(){
                                    abilities = new List<Ability>(){
                                        new ZoneCastAbility(){
                                            activeZoneList = new List<GroupName>(){GroupName.Lost},
                                            cost = new ManaCost("1"),
                                            speed = speed,

                                        }
                                    }
                                }
                            }
                        },
                    }
                };
                var mana = (List<Mana>)cost?.getValueObj(context);
                var ability = new ActivatedZoneAbility(){
                    cost = new ManaCost(mana),
                    id= "Prophesy",
                    speed = SpeedTypes.Action,
                    activeZoneList = new List<GroupName>{GroupName.Hand},
                    effects= new List<EffectScript>(){
                        new SendTo(){
                            targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                            zone = GroupName.Lost,
                        },
                        new Effect.AddModifier(){
                            targeter = new ContextualObjectTargeter(ContextualObjTargets.self),
                            modifiers = new List<BaseModifier>{
                                new TemporaryAbilityModifier(){
                                    abilities = new List<Ability>(){
                                        enabler,
                                    }
                                }
                            }
                        }
                    }
                };
                return ability;
                
            }


        
        }

        [Serializable]
        public class DurationCountdown:ModifierPreset{
            [SerializeReference,SubclassSelector]
            public IValue duration = new Numeric(3);

            public override void applyAsAbility(Card card, Context context)
            {
                if( card.data is ActionCard action){
                    if(duration != null){
                        var num = (int)duration.getValueObj(context) ;
                        Debug.Log(num);
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
                        action.effects.abilities.Add(ETB);
                    }

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
                                    new ActivateTriggerByID(){
                                        ID = new TextString("DurationEnd"),
                                        targeter = new ContextualObjectTargeter(ContextualObjTargets.self)
                                    },
                                    new Destroy(){
                                        targeter = new ContextualObjectTargeter(ContextualObjTargets.self)
                                    },
                                }
                            }
                        },
                        source = card

                    };
                    action.effects.abilities.Add(countdown);
                }
            }
            
        }
    }
    
}