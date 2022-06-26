using System.Globalization;
using BaseProject;
using BaseProject.Utils;
using ModifierSystem;
using UnityEditor;
using UnityEngine;


namespace ModifierController.Editor
{
    [CustomEditor(typeof(GameController))]
    public class TestInEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Test script"))
            {
                /*
                Utilities.TimeTest(delegate
                {
                    statusResistances.GetStatusMultiplier(statusTags);
                }, 100_000,"One: ");

                Utilities.TimeTest(delegate
                {
                    statusResistances.GetStatusMultiplier(statusTagsLong);
                }, 100_000,"Long: ");

                Utilities.TimeTest(delegate
                {
                    statusResistances.GetStatusMultiplier(null);
                }, 100_000,"Null: ");*/
            }
        }
    }
}