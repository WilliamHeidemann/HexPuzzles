using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSpawner))]
public class LevelSpawnerEditor : Editor
{
    private LevelSpawner _script;
    private const int Layers = 10;
    private const float Spacing = 0.2f;
    private static float SideLength => 1f + Spacing;
    private static float Median => SideLength / 1.155f;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _script = (LevelSpawner)target;
        if (GUILayout.Button("Reset Grid"))
        {
            if (_script.assetToLoad != null && _script.assetToUpdate != null)
            {
                Debug.LogAssertion("An asset is still in the update or load field. Remove those before calling Reset Grid.");
                return;
            }
            SpawnHexGrid();
        }
        
        GUILayout.BeginHorizontal();
        var shouldUpdate = _script.assetToUpdate != null;
        var saveOrUpdate = shouldUpdate ? "Update Asset" : "Save to Assets";
        _script.assetToUpdate = (GridScriptableObject)EditorGUILayout.ObjectField("", _script.assetToUpdate, typeof(GridScriptableObject), true);
        if (GUILayout.Button(saveOrUpdate))
        {
            if (shouldUpdate)
            {
                if (_script.assetToUpdate.updatingAllowed == false)
                {
                    Debug.LogAssertion("Level has been locked from updating. Has the correct asset been selected for update?");
                    return;
                }
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
        asset.tileData = AssetDataList();
        const string path = "Assets/New Grid.asset";
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
    }

    private void UpdateAsset()
    {
        _script.assetToUpdate.tileData = AssetDataList();
        EditorUtility.SetDirty(_script.assetToUpdate);
        AssetDatabase.SaveAssets();
    }
    
    private List<AxialHex> AssetDataList()
    {
        var tiles = _script.container.GetComponentsInChildren<LevelTile>();
        var structs = tiles.Where(tile => tile.tileType != TileType.Empty).Select(tile => new AxialHex
        {
            tileType = tile.tileType,
            Q = tile.q,
            R = tile.r,
            teleportQ = TeleportAxialConnection(tile).Item1,
            teleportR = TeleportAxialConnection(tile).Item2,
            switchOn = SwitchOnOrOff(tile)
        });
        return structs.ToList();
    }
    
    private static bool SwitchOnOrOff(LevelTile tile) => tile.TryGetComponent<SwitchTile>(out var switchTile) && switchTile.on;

    private static (int, int) TeleportAxialConnection(LevelTile tile)
    {
        if (!tile.TryGetComponent<TeleportTile>(out var teleportTile)) return (0, 0);
        var teleportLevelTile = teleportTile.connectedTile;
        return (teleportLevelTile.q, teleportLevelTile.r);
    }
    
    private void SpawnHexGrid()
    {
        RemoveOldGrid();
        var axialHexes = GenerateAxialHexes();
        InstantiateGrid(axialHexes);
    }
    
    private void RemoveOldGrid()
    {
        while (_script.container.childCount > 0)
        {
            DestroyImmediate(_script.container.GetChild(0).gameObject);
        }
    }
    
    private static List<AxialHex> GenerateAxialHexes()
    {
        var axialHexes =
            Enumerable.Range(-Layers, Layers * 2 + 1)
                .SelectMany(q => Enumerable.Range(-Layers, Layers * 2 + 1)
                    .SelectMany(r => Enumerable.Range(-Layers, Layers * 2 + 1)
                        .Where(s => q + r + s == 0)
                        .Select(_ => new AxialHex { Q = q, R = r }))).ToList();
        return axialHexes;
    }
    
    private void InstantiateGrid(List<AxialHex> axialHexes)
    {
        var qVector = new Vector3(Median * 2, 0, 0);
        var rVector = new Vector3(Median, 0, SideLength * 1.5f);
        foreach (var axialHex in axialHexes)
        {
            var qPos = qVector * axialHex.Q;
            var rPos = rVector * axialHex.R;
            var position = qPos + rPos;
            var tile = Instantiate(_script.tilePrefab, position, Quaternion.identity, _script.container);
            // var tile = (LevelTile)PrefabUtility.InstantiatePrefab(_script.tilePrefab, _script.container);
            // tile.transform.position = position;
            tile.ApplyTileStruct(axialHex);
            tile.name = $"Q({axialHex.Q}) R({axialHex.R})";
        }
    }
}
