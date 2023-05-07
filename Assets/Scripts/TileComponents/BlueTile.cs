using UnityEngine;

public class BlueTile : TileComponentBase, IActivatedTile
{
    public static int BlueTilesInLevel;
    public void Activate()
    {
        BlueTilesInLevel--;
        var levelTile = GetComponent<LevelTile>();
        levelTile.tileType = TileType.Standard;
        levelTile.UpdateGraphics();
        Destroy(this);
    }
}