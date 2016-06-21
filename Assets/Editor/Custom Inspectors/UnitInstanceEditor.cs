using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using AI_vs_I.Units;

namespace Editor.Custom_Inspectors
{

    [CustomEditor(typeof(UnitBody))]
    public class UnitInstanceEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            UnitBody myTargetInst = (UnitBody)target;
            UnitInstance myTarget = myTargetInst.UnitInstance;

            EditorGUILayout.EnumPopup("Owner", myTarget.Owner);
            EditorGUILayout.IntField("HP", myTarget.CurrentHealth);
            EditorGUILayout.IntField("History", myTarget.MoveHistory.Count);
        }

    }

}
