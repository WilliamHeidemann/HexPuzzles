using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public static event Action PlayerMoved;
    private static IEnumerable<LevelTile> LevelTiles => FindObjectsOfType<LevelTile>();
    private const int ViewDistance = 2;
    [HideInInspector] public LevelTile previous;
    [HideInInspector] public LevelTile current;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        LevelLoader.EnterLevelEvent += CenterPlayer;
    }

    private void OnDestroy()
    {
        LevelLoader.EnterLevelEvent -= CenterPlayer;
    }

    private void CenterPlayer(GridScriptableObject level)
    {
        previous = null;
        current = LevelTiles.First(tile => tile.q == 0 && tile.r == 0);
        DisplayTilesInRange();
        transform.position = current.transform.position + Vector3.up;
    }

    public void MoveRequest(LevelTile tile)
    {
        if (IsInvalid(tile)) return;
        SendToNewTile(tile);
        if (tile.TryGetComponent<IActivatedTile>(out var activatedTile)) activatedTile.Activate();
        PlayerMoved?.Invoke();
        StepCounter.Instance.IncrementStepCount();
        ObjectiveManager.Instance.ProgressionCheck();
    }

    public void TeleportRequest(LevelTile tile)
    {
        SendToNewTile(tile);
        ObjectiveManager.Instance.ProgressionCheck();
        // Replace with fade?
        // Display all tiles while flying?
    }
    
    private bool IsInvalid(LevelTile tile)
    {
        if (ObjectiveManager.Instance.LevelComplete) return true;
        if (!InRange(current, tile, 1)) return true;
        if (tile.tileType == TileType.Empty) return true;
        if (tile.tileType == TileType.Switch && !tile.GetComponent<SwitchTile>().on) return true;
        if (tile == current) return true;
        return false;
    }

    private void SendToNewTile(LevelTile tile)
    {
        previous = current;
        current = tile;
        PlayerMovementAnimation.Instance.AddMovementCommand(tile.transform.position + Vector3.up);
    }
    
    public void DisplayTilesInRange()
    {
        foreach (var tile in LevelTiles)
        {
            if (InRange(current, tile, ViewDistance) || InRange(previous, tile, ViewDistance))
            {
                tile.UpdateGraphics();
            }
            else
            {
                tile.TurnTransparent();
            }
        }
    }
    
    private static bool InRange(LevelTile first, LevelTile second, int range)
    {
        if (first == null || second == null) return false;
        var qDiff = Mathf.Abs(first.q - second.q);
        var rDiff = Mathf.Abs(first.r - second.r);
        var sDiff = Mathf.Abs(first.s - second.s);
        var distance = Mathf.Max(qDiff, rDiff, sDiff);
        return distance <= range;
    }
}
