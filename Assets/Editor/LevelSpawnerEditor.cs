using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSpawner))]
public class LevelSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (LevelSpawner)target;
        if (GUILayout.Button("Reset Grid"))
        {
            script.SpawnHexGrid();
        }
        
        GUILayout.BeginHorizontal();
        var shouldUpdate = script.assetToUpdate != null;
        var saveOrUpdate = shouldUpdate ? "Update Asset" : "Save to Assets";
        script.assetToUpdate = (GridScriptableObject)EditorGUILayout.ObjectField("", script.assetToUpdate, typeof(GridScriptableObject), true);
        if (GUILayout.Button(saveOrUpdate))
        {
            if (shouldUpdate)
            {
                script.UpdateAsset();
                script.assetToLoad = script.assetToUpdate;
                script.LoadAsset();
            }
            else
            {
                script.SaveToAssets();
                script.assetToLoad = script.assetToUpdate;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        script.assetToLoad = (GridScriptableObject)EditorGUILayout.ObjectField("", script.assetToLoad, typeof(GridScriptableObject), true);
        if (GUILayout.Button("Load Asset"))
        {
            script.LoadAsset();
            script.assetToUpdate = script.assetToLoad;
        }
        GUILayout.EndHorizontal();
    }
}
