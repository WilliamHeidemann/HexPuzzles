using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private static IEnumerable<LevelTile> LevelTiles => FindObjectsOfType<LevelTile>();
    private const int ViewDistance = 2;
    private LevelTile _previous;
    [HideInInspector] public LevelTile current;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
        LevelLoader.EnterLevelEvent += CenterPlayer;
    }

    private void CenterPlayer(GridScriptableObject level)
    {
        _previous = null;
        current = LevelTiles.First(tile => tile.q == 0 && tile.r == 0);
        DisplayTilesInRange();
        transform.position = current.transform.position + Vector3.up;
    }

    public void MoveRequest(LevelTile tile)
    {
        if (ObjectiveManager.Instance.LevelComplete) return;
        if (!InRange(current, tile, 1)) return;
        if (tile.tileType == TileType.Empty) return;
        if (tile == current) return;
        PlayerMovementAnimation.Instance.AddMovementCommand(tile.transform.position + Vector3.up);
        _previous = current;
        current = tile;
        if (tile.TryGetComponent<IActivatedTile>(out var activatedTile)) activatedTile.Activate();
        StepCounter.Instance.IncrementStepCount();
        ObjectiveManager.Instance.ProgressionCheck();
    }

    public void Teleport(LevelTile tile)
    {
        PlayerMovementAnimation.Instance.AddMovementCommand(tile.transform.position + Vector3.up); // Replace with fade?
        current = tile;
    }
    
    public void DisplayTilesInRange()
    {
        foreach (var tile in LevelTiles)
        {
            if (InRange(current, tile, ViewDistance) || InRange(_previous, tile, ViewDistance))
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
