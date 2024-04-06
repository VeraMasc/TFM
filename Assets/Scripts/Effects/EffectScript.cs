using System;
using System.Collections;
using System.Reflection;
using CardHouse;
using UnityEngine;

[Serializable]
public abstract class EffectScript: IClonableEffectElement
{
    /// <summary>
    /// Executes the effect
    /// </summary>
    public virtual IEnumerator execute(CardResolveOperator stack, TargettingContext context){
        throw new NotImplementedException();
        
    }
 
    public IClonableEffectElement clone(){
        return cloneScriptObj(this);
    }

    /// <summary>
    /// Crea un clon idéntico del effect script. Sobreescribir para hacer copia profunda
    /// </summary>
    public static IClonableEffectElement cloneScriptObj(IClonableEffectElement source){
        //step : 1 Get the type of source object and create a new instance of that type
        Type typeSource = source.GetType();
        object objTarget = Activator.CreateInstance(typeSource);
        //Step2 : Get all the properties of source object type
        PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //Step : 3 Assign all source property to taget object 's properties
        foreach (PropertyInfo property in propertyInfo)
        {
            //Check whether property can be written to
            if (property.CanWrite)
            {
                //Step : 4 check whether property type should be cloned or not
                if (property.PropertyType.IsAssignableFrom(typeof(IClonableEffectElement)) || property.PropertyType.IsAssignableFrom(typeof(IEnumerable)))
                {
                    
                    object objPropertyValue = property.GetValue(source, null);
                    if (objPropertyValue == null)
                    {
                        property.SetValue(objTarget, null, null);
                    }
                    else
                    {
                        property.SetValue(objTarget, cloneScriptObj(objPropertyValue as IClonableEffectElement), null);
                    }
                   
                }
                else //Clonado superficial
                {
                    property.SetValue(objTarget, property.GetValue(source, null), null);
                }
            }
        }
        return objTarget as EffectScript;
        
    }

    
}

/// <summary>
/// Marca los elementos de la cadena de scripts que se han de clonar en profundidad
/// </summary>
public interface IClonableEffectElement{
    public IClonableEffectElement clone();
}