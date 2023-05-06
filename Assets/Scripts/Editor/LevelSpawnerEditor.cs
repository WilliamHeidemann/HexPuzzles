using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSpawner))]
public class LevelSpawnerEditor : Editor
{
    private LevelSpawner _script;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _script = (LevelSpawner)target;
        if (GUILayout.Button("Reset Grid"))
        {
            _script.SpawnHexGrid();
        }
        
        GUILayout.BeginHorizontal();
        var shouldUpdate = _script.assetToUpdate != null;
        var saveOrUpdate = shouldUpdate ? "Update Asset" : "Save to Assets";
        _script.assetToUpdate = (GridScriptableObject)EditorGUILayout.ObjectField("", _script.assetToUpdate, typeof(GridScriptableObject), true);
        if (GUILayout.Button(saveOrUpdate))
        {
            if (shouldUpdate)
            {
                UpdateAsset();
                _script.assetToLoad = _script.assetToUpdate;
                _script.LoadAsset(_script.assetToLoad);
            }
            else
            {
                SaveToAssets();
                _script.assetToLoad = _script.assetToUpdate;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        _script.assetToLoad = (GridScriptableObject)EditorGUILayout.ObjectField("", _script.assetToLoad, typeof(GridScriptableObject), true);
        if (GUILayout.Button("Load Asset"))
        {
            _script.LoadAsset(_script.assetToLoad);
            _script.assetToUpdate = _script.assetToLoad;
        }
        GUILayout.EndHorizontal();
    }

    private void SaveToAssets()
    {
        var asset = CreateInstance<GridScriptableObject>();
        asset.tileData = _script.AssetDataList();
        const string path = "Assets/New Grid.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
    }

    private void UpdateAsset()
    {
        _script.assetToUpdate.tileData = _script.AssetDataList();
        EditorUtility.SetDirty(_script.assetToUpdate);
        AssetDatabase.SaveAssets();
    }
}
