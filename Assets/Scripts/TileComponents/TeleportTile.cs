using UnityEngine;

public class TeleportTile : TileComponentBase, IActivatedTile
{
    public LevelTile connectedTile;
    public void Activate()
    {
        PlayerMovement.Instance.TeleportRequest(connectedTile);
    }
}