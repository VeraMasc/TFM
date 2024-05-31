using System;
using System.Collections;
using System.Collections.Generic;
using CardHouse;
using Common.Coroutines;
using UnityEngine;


namespace Effect{
    /// <summary>
    /// Habilidad que da a la carta unas habilidades marcadas por un preset
    /// </summary>
    [Serializable]
    public class PresetAbility : Ability
    {
        [SerializeReference, SubclassSelector]
        public List<Preset.ModifierPreset> presets;
        public override void onChangeZone(GroupName zone)
        {
            if(source.data is MyCardSetup setup){
                setup.effects.setContext(source);
                if(setup.effects.abilities.Remove(this)){
                    applyPresets(setup.effects.context);
                }
            }
        }

        private void applyPresets(Context context){
            foreach(var preset in presets){
                preset.applyAsAbility(source,context);
            }
        }
    }

}