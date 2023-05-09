using UnityEngine;

public class TeleportTile : TileComponentBase, IActivatedTile
{
    public LevelTile connectedTile;
    public void Activate()
    {
        PlayerMovement.Instance.MoveRequest(new MoveCommand(connectedTile, MoveType.Teleport));
    }
}