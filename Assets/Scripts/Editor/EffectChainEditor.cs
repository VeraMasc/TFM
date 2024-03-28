using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MackySoft.SerializeReferenceExtensions.Editor;
using Unity.VisualScripting;

[CustomPropertyDrawer(typeof(EffectChain))]
public class EffectChainEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        displayChain(position,property,label);

		// EditorGUILayout.PropertyField(serializedObject.FindProperty("effect"));
        // EditorGUILayout.PropertyField(serializedObject.FindProperty("next"));
        EditorGUI.EndProperty();
		
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var effect = property.FindPropertyRelative("effect");
        var myHeight = EditorGUI.GetPropertyHeight(effect,true);
        var next = property.FindPropertyRelativeOrFail("next");
        var contentHeight = next?.managedReferenceValue != null? EditorGUI.GetPropertyHeight(next, label, true): EditorGUIUtility.singleLineHeight;
        return myHeight + contentHeight;
    }

    /// <summary>
    /// Recursive display
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public void displayChain(Rect position, SerializedProperty property, GUIContent label){
        var horizontal = EditorGUILayout.BeginHorizontal();
        var effect = property.FindPropertyRelative("effect");
        //Get height
        horizontal.height = EditorGUI.GetPropertyHeight(effect,true);
        horizontal.yMin=position.yMin;
        Debug.Log(horizontal.height);
        var effectRect = new Rect(horizontal);
        effectRect.xMax -= 30;
        
        EditorGUI.PropertyField(effectRect,effect);
        // effect.stringValue = EditorGUI.TextField(effectRect, "Effect",effect.stringValue);
        var buttonRect = new Rect(horizontal);
        buttonRect.xMin = buttonRect.xMax -20;
        //Get next
        var next = property.FindPropertyRelativeOrFail("next");

        //Delete button
        if(next.managedReferenceValue!=null && GUI.Button(buttonRect,new GUIContent("x", $"Delete this effect"),GUI.skin.box)){
            next.managedReferenceValue = null;
        }
        EditorGUILayout.EndHorizontal();

        //Recursive loop
        if(next?.managedReferenceValue != null || effect?.exposedReferenceValue != null){ 
           
            //DrawProperties()
            next.managedReferenceValue ??= new EffectChain();
            var nextRect = new Rect(position.x, position.y + 20f, position.width, EditorGUIUtility.singleLineHeight);
            displayChain(nextRect, next, label);
        }
    }
}