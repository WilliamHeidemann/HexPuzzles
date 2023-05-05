using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelTile))]
[CanEditMultipleObjects]
public class LevelTileEditor : Editor
{
    private LevelTile _tile;
    public void OnValidate()
    {
        _tile = (LevelTile)target;
        UpdateTileComponent();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        _tile = (LevelTile)target;
        UpdateTileComponent();
    }

    private void UpdateTileComponent()
    {
        switch (_tile.tileType)
        {
            case TileType.Empty:
                UpdateTileComponentContainingNothing();
                break;
            case TileType.Standard:
                UpdateTileComponentContainingNothing();
                break;
            case TileType.Blue:
                UpdateTileComponentContaining<BlueTile>();
                break;
            case TileType.Teleport:
                UpdateTileComponentContaining<TeleportTile>();
                break;
            case TileType.BonusSteps:
                UpdateTileComponentContaining<BonusStepsTile>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateTileComponentContainingNothing()
    {
        var activatedTiles = _tile.GetComponents<IActivatedTile>();
        foreach (var tile in activatedTiles)
        {
            EditorApplication.delayCall += () => DestroyImmediate((Component)tile);
        }
    }

    private void UpdateTileComponentContaining<T>() where T : Component
    {
        var activatedTiles = _tile.GetComponents<IActivatedTile>();
        foreach (var tile in activatedTiles)
        {
            if (tile is not T)
            {
                EditorApplication.delayCall += () => DestroyImmediate((Component)tile);
            }
        }
        if (_tile.GetComponent<T>() is null) _tile.gameObject.AddComponent<T>();
    }
}
