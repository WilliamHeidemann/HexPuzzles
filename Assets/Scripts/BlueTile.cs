using UnityEngine;

public class BlueTile : MonoBehaviour, IActivatedTile
{
    public void Activate()
    {
        ObjectiveManager.Instance.ProgressObjective();
        var levelTile = GetComponent<LevelTile>();
        levelTile.tileType = TileType.Standard;
        levelTile.UpdateGraphics();
        Destroy(this);
    }
}