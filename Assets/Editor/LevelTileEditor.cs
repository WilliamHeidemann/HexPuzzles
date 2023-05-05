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
        var blueTile = _tile.GetComponent<BlueTile>();
        if (blueTile is not null)
        {
            EditorApplication.delayCall += () => DestroyImmediate(blueTile);
        }
        
        var teleportTile = _tile.GetComponent<TeleportTile>();
        if (teleportTile is not null)
        {
            EditorApplication.delayCall += () => DestroyImmediate(teleportTile);
        }
        
        var bonusStepsTile = _tile.GetComponent<BonusStepsTile>();
        if (bonusStepsTile is not null)
        {
            EditorApplication.delayCall += () => DestroyImmediate(bonusStepsTile);
        }
    }

    private void UpdateTileComponentContaining<T>() where T : Component
    {
        var typeToKeep = _tile.GetComponent(typeof(T));
        
        if (typeToKeep is not BlueTile)
        {
            var blueTile = _tile.GetComponent<BlueTile>();
            if (blueTile is not null)
            {
                EditorApplication.delayCall += () => DestroyImmediate(blueTile);
            }
        }

        if (typeToKeep is not TeleportTile)
        {
            var teleportTile = _tile.GetComponent<TeleportTile>();
            if (teleportTile != null)
            {
                EditorApplication.delayCall += () => DestroyImmediate(teleportTile);
            }
        }
        
        if (typeToKeep is not BonusStepsTile)
        {
            var bonusStepsTile = _tile.GetComponent<BonusStepsTile>();
            if (bonusStepsTile != null)
            {
                EditorApplication.delayCall += () => DestroyImmediate(bonusStepsTile);
            }
        }

        if (typeToKeep is null)
        {
            _tile.gameObject.AddComponent<T>();
        }
    }
}
