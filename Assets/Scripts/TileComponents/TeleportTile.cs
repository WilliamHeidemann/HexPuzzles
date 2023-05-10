using UnityEngine;

public class TeleportTile : TileComponentBase, IActivatedTile
{
    public LevelTile connectedTile;
    public void Activate()
    {
        PlayerMovement.Instance.MoveRequest(new MoveCommand(connectedTile, shouldTriggerTileEvent: false, shouldActivateTile: false, shouldIncrementStepCount: false, shouldCheckRange:false, shouldDisplayAllTiles: true));
    }
}