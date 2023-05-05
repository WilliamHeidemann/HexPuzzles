using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelTile))]
[CanEditMultipleObjects]
public class LevelTileEditor : Editor
{
    public void OnSceneGUI()
    {
        Tools.hidden = false;
    }
}
