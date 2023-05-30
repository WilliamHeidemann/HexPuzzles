using UnityEngine;

namespace GamePlay
{
    public enum AnimationType
    {
        Hop,
        Dig
    }
    public class MoveCommand
    {
        public LevelTile Tile { get; }
        public Vector3 Position { get; }
        public AnimationType AnimationType { get; }
        public bool ShouldTriggerTileEvent { get; }
        public bool ShouldActivateTile { get; }
        public bool ShouldIncrementStepCount { get; }
        public bool ShouldCheckRange { get; }
        public MoveCommand(LevelTile levelTile, AnimationType animationType = AnimationType.Hop, bool shouldTriggerTileEvent = true, bool shouldActivateTile = true, bool shouldIncrementStepCount = true, bool shouldCheckRange = true)
        {
            Tile = levelTile;
            Position = levelTile.transform.position + new Vector3(0f, 0.25f, 0f);
            AnimationType = animationType;
            ShouldTriggerTileEvent = shouldTriggerTileEvent;
            ShouldActivateTile = shouldActivateTile;
            ShouldIncrementStepCount = shouldIncrementStepCount;
            ShouldCheckRange = shouldCheckRange;
        }
    }
}