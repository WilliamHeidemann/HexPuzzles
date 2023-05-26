using System;
using GamePlay;
using TileComponents;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelTile))]
[CanEditMultipleObjects]
public class LevelTileEditor : Editor
{
    private LevelTile _tile;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _tile = (LevelTile)target;
        UpdateTileComponent();
    }

    private void UpdateTileComponent()
    {
        var componentToKeep = _tile.GetTileComponent(_tile.tileType);
        var tileComponents = _tile.GetComponents<TileComponentBase>();
        foreach (var component in tileComponents)
        {
            if (component != componentToKeep)
            {
                EditorApplication.delayCall += () => DestroyImmediate(component);
            }
        }
    }
}