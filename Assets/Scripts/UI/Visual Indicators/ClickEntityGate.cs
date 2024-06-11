using System.Linq;
using CardHouse;
using CustomInspector;
using UnityEngine;

public class ClickEntityGate : Gate<NoParams>
{
    [SelfFill]
    public TargetDetector detector;
    protected override bool IsUnlockedInternal(NoParams argObject)
    {
        var UI = GameUI.singleton;
        if(UI.possibleTargets != null)
            Debug.Log(string.Join(", ",UI.possibleTargets));
        return UI.possibleTargets== null || !detector.isValid();
    }
}