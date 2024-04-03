using UnityEngine;

public abstract class CardHelp : ScriptableObject
{
    

    public virtual void displayHelp(GameObject obj){
        var classname= this.GetType().Name;
        Debug.Log($"Help class {classname} not implemented");
    }
}