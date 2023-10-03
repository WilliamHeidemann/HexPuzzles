#if UNITY_EDITOR

using GamePlay;
using TileComponents;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TeleportTile))]
[CanEditMultipleObjects]
public class TeleportEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Selection.count != 2) return;
        if (GUILayout.Button("Connect Tiles"))
        {
            var tileOne = Selection.gameObjects[0].GetComponent<TeleportTile>();
            var tileTwo = Selection.gameObjects[1].GetComponent<TeleportTile>();
            tileOne.connectedTile = tileTwo.GetComponent<LevelTile>();
            tileTwo.connectedTile = tileOne.GetComponent<LevelTile>();
        }
    }
}

#endif