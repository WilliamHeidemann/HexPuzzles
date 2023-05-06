using UnityEngine;

public class BonusStepsTile : MonoBehaviour, IActivatedTile
{
    public void Activate()
    {
        StepCounter.Instance.GainBonusSteps(4);
        var levelTile = GetComponent<LevelTile>();
        levelTile.tileType = TileType.Standard;
        levelTile.UpdateGraphics();
        Destroy(this);
    }
}