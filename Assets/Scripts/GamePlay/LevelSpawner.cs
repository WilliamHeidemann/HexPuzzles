using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public LevelTile tilePrefab;
    [HideInInspector] public GridScriptableObject assetToUpdate;
    [HideInInspector] public GridScriptableObject assetToLoad;
    public Transform container;
    public float spacing;
    public int layers = 4;
    private float SideLength => 1f + spacing;
    private float Median => SideLength / 1.155f;

    private void Awake()
    {
        LevelLoader.EnterLevelEvent += SetAssetToLoad;
        LevelLoader.EnterLevelEvent += LoadAsset;
    }

    private void OnDestroy()
    {
        LevelLoader.EnterLevelEvent -= SetAssetToLoad;
        LevelLoader.EnterLevelEvent -= LoadAsset;
    }

    private void SetAssetToLoad(GridScriptableObject level)
    {
        assetToLoad = level;
    }

    public void SpawnHexGrid()
    {
        RemoveOldGrid();
        var axialHexes = GenerateAxialHexes();
        InstantiateGrid(axialHexes);
    }

    public void LoadAsset(GridScriptableObject level)
    {
        if (level == null) return;
        var hexTiles = container.GetComponentsInChildren<LevelTile>();
        // Clear Old Content
        foreach (var levelTile in hexTiles)
        {
            levelTile.ApplyTileStruct(new AxialHex
            {
                tileType = TileType.Empty,
                Q = levelTile.q,
                R = levelTile.r,
            });
        }
        
        // Add New Content
        foreach (var tileStruct in level.tileData)
        {
            var hexTile = hexTiles.FirstOrDefault(hexTile => hexTile.q == tileStruct.Q && hexTile.r == tileStruct.R);
            if (hexTile != null) hexTile.ApplyTileStruct(tileStruct);
        }
    }

    public List<AxialHex> AssetDataList()
    {
        var tiles = container.GetComponentsInChildren<LevelTile>();
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

    private bool SwitchOnOrOff(LevelTile tile) => tile.TryGetComponent<SwitchTile>(out var switchTile) && switchTile.on;

    private (int, int) TeleportAxialConnection(LevelTile tile)
    {
        if (!tile.TryGetComponent<TeleportTile>(out var teleportTile)) return (0, 0);
        var teleportLevelTile = teleportTile.connectedTile;
        return (teleportLevelTile.q, teleportLevelTile.r);
    }

    private void RemoveOldGrid()
    {
        while (container.childCount > 0)
        {
            DestroyImmediate(container.GetChild(0).gameObject);
        }
    }
    
    private List<AxialHex> GenerateAxialHexes()
    {
        var axialHexes =
            Enumerable.Range(-layers, layers * 2 + 1)
                .SelectMany(q => Enumerable.Range(-layers, layers * 2 + 1)
                    .SelectMany(r => Enumerable.Range(-layers, layers * 2 + 1)
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
            var tile = Instantiate(tilePrefab, position, Quaternion.identity, container);
            tile.ApplyTileStruct(axialHex);
            tile.name = $"Q({axialHex.Q}) R({axialHex.R})";
        }
    }
}