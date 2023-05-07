using System;
using UnityEngine;

public class SwitchTile : TileComponentBase, IEventTriggerTile
{
    private LevelTile _levelTile;
    public bool on;
    
    private void Awake()
    {
        PlayerMovement.PlayerMoved += Trigger;
    }

    private void OnDestroy()
    {
        PlayerMovement.PlayerMoved -= Trigger;
    }
    
    private void OnValidate()
    {
        GetComponent<LevelTile>().UpdateGraphics();
    }

    private void Start()
    {
        _levelTile = GetComponent<LevelTile>();
    }
    

    public void Trigger()
    {
        if (PlayerMovement.Instance.current == _levelTile) return;
        on = !on;
        _levelTile.UpdateGraphics();
    }
}