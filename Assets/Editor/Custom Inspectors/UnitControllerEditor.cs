using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using AI_vs_I.Units;

namespace Editor.Custom_Inspectors
{

    [CustomEditor(typeof(UnitController))]
    public class UnitControllerEditor : UnityEditor.Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            UnitController myTarget = (UnitController)target;

            if (GUILayout.Button("Energize")) { myTarget.Energize(); }
        }

    }

}
