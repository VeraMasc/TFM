using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MackySoft.SerializeReferenceExtensions.Editor;
using Unity.VisualScripting;
using GameFlow;

[CustomPropertyDrawer(typeof(EffectChain))]
public class EffectChainEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var list = property.FindPropertyRelative("list");
        return EditorGUI.GetPropertyHeight(list,label,true);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        var list = property.FindPropertyRelative("list");
        EditorGUI.PropertyField(position,list,label,true);
    }
   
}