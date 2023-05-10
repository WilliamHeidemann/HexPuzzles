using System;
using System.Linq;
using UnityEngine;

internal enum TileDirection
{
    Right,
    LowerRight,
    LowerLeft,
    Left,
    UpperLeft,
    UpperRight
}

public class RotatingTile : TileComponentBase, IActivatedTile, IEventTriggerTile
{
    [SerializeField] private TileDirection direction;
    [SerializeField] [Range(1,3)] private int rotationsPerMove = 1;
    [SerializeField] private bool counterClockwiseRotation;
    private LevelTile LevelTile => GetComponent<LevelTile>();
    private readonly int[] _qr = { -1, 0, 1, 1, 0, -1};
    private int qIndex = 0;
    private int rIndex = 5;
    private int targetQ => LevelTile.q + _qr[qIndex];
    private int targetR => LevelTile.r + _qr[rIndex];
    private LevelTile TilePointedTo =>
        FindObjectsOfType<LevelTile>().First(tile => tile.q == targetQ && tile.r == targetR);
    
    private void Awake()
    {
        PlayerMovement.TriggerTileEvent += Trigger;
    }

    private void RotationLogic()
    {
        qIndex = (qIndex + rotationsPerMove) % 6;
        rIndex = (rIndex + rotationsPerMove) % 6;
    }

    public void Activate()
    {
        PlayerMovement.Instance.MoveRequest(new MoveCommand(TilePointedTo, MoveType.Walk));
    }

    public void Trigger()
    {
        RotationLogic();
        // Play animation
    }
}

// q, r = 0, 0 => middle
// -1, 0 => left
// 0, -1 => upper left
// +1, -1 => upper right
// +1, 0 => right
// 0, +1 => lower right
// -1, +1 => lower left