using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleMan.GlobalEvents.Core
{
    //Simple
    [CustomEditor(typeof(Handler), true)]
    public class HandlerSimpleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Invoke", GUILayout.Height(30)))
            {
                Handler owner = target as Handler;
                owner.Invoke(owner);
            }
        }
    }

    [CustomEditor(typeof(VoteSimple), true)]
    public class VoteSimpleEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Begin vote", GUILayout.Height(30)))
            {
                VoteSimple owner = target as VoteSimple;
                owner.BeginVote(owner);
            }
        }
    }
}