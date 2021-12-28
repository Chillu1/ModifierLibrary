using System.Collections.Generic;
using BaseProject;
using ModifierSystem;
using UnityEditor;
using UnityEngine;

namespace ModifierController.Editor
{
    public class ControlWindow : EditorWindow
    {
        string myString = "Hello World";
        bool groupEnabled;
        bool myBool = true;
        float myFloat = 1.23f;

        private readonly List<Being> _beings = new List<Being>();

        [MenuItem("Window/ModifierSystem Control")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ControlWindow));
        }

        void OnGUI()
        {
            GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
            //EditorGUILayout.ta
            myString = EditorGUILayout.TextField ("Text Field", myString);

            if (GUILayout.Button("Get all beings"))
            {
                _beings.Clear();
                var gameController = FindObjectOfType<GameController>();
                _beings.Add(gameController.player);
                _beings.Add(gameController.enemy);
            }

            string text = "";
            if(_beings.Count > 0)
                text = _beings[0].ToString();
            EditorGUILayout.TextField(text);

            groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
            myBool = EditorGUILayout.Toggle ("Toggle", myBool);
            myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);

            EditorGUILayout.EndToggleGroup ();
        }
    }
}