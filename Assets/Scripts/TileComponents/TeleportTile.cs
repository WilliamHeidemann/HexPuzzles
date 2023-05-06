using UnityEngine;

public class TeleportTile : MonoBehaviour, IActivatedTile
{
    public LevelTile connectedTile;
    public void Activate()
    {
        PlayerMovement.Instance.TeleportRequest(connectedTile.GetComponent<LevelTile>());
    }
}