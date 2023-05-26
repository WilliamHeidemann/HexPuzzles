using UnityEngine;

namespace GamePlay
{
    public enum MoveAnimation
    {
        Walk,
        Jump,
        Teleport,
        Rotate
    }

    public class MoveCommand
    {
        public LevelTile Tile { get; }
        public Vector3 Position { get; }
        public bool ShouldTriggerTileEvent { get; }
        public bool ShouldActivateTile { get; }
        public bool ShouldIncrementStepCount { get; }
        public bool ShouldCheckRange { get; }
        public bool ShouldDisplayAllTiles { get; }
        public MoveCommand(LevelTile levelTile, bool shouldTriggerTileEvent = true, bool shouldActivateTile = true, bool shouldIncrementStepCount = true, bool shouldCheckRange = true, bool shouldDisplayAllTiles = false)
        {
            Tile = levelTile;
            Position = levelTile.transform.position + new Vector3(0f, 0.25f, 0f);
            ShouldTriggerTileEvent = shouldTriggerTileEvent;
            ShouldActivateTile = shouldActivateTile;
            ShouldIncrementStepCount = shouldIncrementStepCount;
            ShouldCheckRange = shouldCheckRange;
            ShouldDisplayAllTiles = shouldDisplayAllTiles;
        }
    }
}