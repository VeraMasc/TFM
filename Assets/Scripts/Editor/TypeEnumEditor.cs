using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(CardTypeEnum))]
public class TypeEnumEditor : PropertyDrawer
{

    int _choiceIndex;
    string valueText="";

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label)*2.5f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty typeNameProperty = property.FindPropertyRelative("value");
        
        EditorGUI.BeginChangeCheck();

        //TODO: Finish dropdown
       
        var dropPos = new Rect(position);
        dropPos.height/=2.5f;
        _choiceIndex = EditorGUI.Popup( dropPos, _choiceIndex, CardTypeEnum.options.ToArray() );

        var textpos = new Rect(position);
        textpos.height/=2.5f;
        textpos.y+= position.height-textpos.height;
        valueText = EditorGUI.TextField (textpos, valueText);

       if( EditorGUI.EndChangeCheck() )
        {
            if(CardTypeEnum.options[_choiceIndex] != typeNameProperty.stringValue){ //Index changed
                if(_choiceIndex == 0){ //Null value
                    typeNameProperty.stringValue = valueText = "";
                }else{
                    typeNameProperty.stringValue = valueText = CardTypeEnum.options[_choiceIndex];
                }
            }
            else{ //Text changed
                if(_choiceIndex != 0){
                    _choiceIndex = 0;
                }
            }
            

           
        }
        
        
    }
}