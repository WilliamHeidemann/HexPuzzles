using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public static event Action TriggerTileEvent;
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
    
    // Any type of move (walk, jump, teleport)
    // Check tile is valid (player may click invalid tile. jump may incorrectly be placed)
    // previous = current, current = tile
    // Start translating towards tile and animate character (according to walk/jump/teleport)
    // While traveling, display the tiles around the tiles moved over. Maybe find the shortest path with BFS.
    // Once reached, Trigger tile events (walked there only)
    // Once reached, Activate tile (walked/jumped there only)
    // Once reached, Increment Step count (walked there only)
    // Once reached, Check if game is over (irrelevant for teleport)
    // Once reached, Display tiles in range
    public void MoveRequest(MoveCommand command)
    {
        if (IsInvalid(command)) return;
        previous = current;
        current = command.Tile;
        PlayerMovementAnimation.Instance.MoveTo(command);
    }

    public void MoveRequestCompleted(MoveCommand command)
    {
        if (command.MoveType is MoveType.Walk) TriggerTileEvent?.Invoke();
        if (command.MoveType is MoveType.Walk or MoveType.Jump)
            if (command.Tile.TryGetComponent<IActivatedTile>(out var activatedTile)) 
                activatedTile.Activate();
        if (command.MoveType is MoveType.Walk) StepCounter.Instance.IncrementStepCount();
        DisplayTilesInRange(); // Should take tile argument because current might change before, due to activate
        ObjectiveManager.Instance.ProgressionCheck();
    }
    
    private bool IsInvalid(MoveCommand command)
    {
        if (ObjectiveManager.Instance.LevelComplete) return true;
        if (command.Tile.tileType == TileType.Empty) return true;
        if (command.Tile.tileType == TileType.Switch && !command.Tile.GetComponent<SwitchTile>().on) return true;
        if (command.Tile == current) return true;
        if (command.MoveType is MoveType.Walk)
            if (!InRange(current, command.Tile, 1)) return true;
        return false;
    }

    private void DisplayTilesInRange()
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
