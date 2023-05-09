using UnityEngine;

public enum MoveType
{
    Walk,
    Jump,
    Teleport
} 

public class MoveCommand
{
    public LevelTile Tile { get; }
    public MoveType MoveType { get; }
    public Vector3 Position { get; }
    public MoveCommand(LevelTile levelTile, MoveType moveType)
    {
        MoveType = moveType;
        Tile = levelTile;
        Position = levelTile.transform.position + Vector3.up;
    }
}
