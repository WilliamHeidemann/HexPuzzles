#if UNITY_EDITOR

using System.Collections.Generic;
using MainMenu;
using ScriptableObjectClasses;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveDataModifier))]
public class SaveDataModifierEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (SaveDataModifier)target;
        if (GUILayout.Button("Reset Save Data"))
        {
            script.worldIndexReached = 0;
            PlayerPrefs.DeleteAll();
        }
        
        if (GUILayout.Button("Complete All Levels"))
        {
            foreach (var level in script.levels)
            {
                PlayerPrefs.SetInt("World Reached", 5);
                script.worldIndexReached = 5;
                PlayerPrefs.SetInt(level.name, 3);
            }
        }
        
        if (GUILayout.Button("Randomize Progression"))
        {
            var worldReached = Random.Range(0, 6);
            script.worldIndexReached = worldReached;
            PlayerPrefs.SetInt("World Reached", worldReached);
            foreach (var level in script.levels)
            {
                var stars = Random.Range(0, 4);
                PlayerPrefs.SetInt(level.name, stars);
            }
        }
        
        if (GUILayout.Button("Complete World"))
        {
            var world = script.worldToComplete;
            var center = world.centerLevel;
            var outer = world.connectedLevels;
            var levels = new List<GridScriptableObject> { center };
            levels.AddRange(outer);
            levels.ForEach(level => PlayerPrefs.SetInt(level.name, 3));
            PlayerPrefs.SetInt("World Reached", world.index + 1);
            script.worldIndexReached = world.index + 1;
        }
    }
}

#endif