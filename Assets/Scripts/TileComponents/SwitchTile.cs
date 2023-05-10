﻿using System;
using UnityEngine;

public class SwitchTile : TileComponentBase, IEventTriggerTile
{
    public bool on;
    
    private void Awake()
    {
        PlayerMovement.TriggerTileEvent += Trigger;
    }

    private void OnDestroy()
    {
        PlayerMovement.TriggerTileEvent -= Trigger;
    }
    
    private void OnValidate()
    {
        GetComponent<LevelTile>().UpdateGraphics();
    }

    public void Trigger() => Switch();
    private void Switch()
    {
        var levelTile = GetComponent<LevelTile>();
        if (PlayerMovement.Instance.current == levelTile) return;
        on = !on;
        levelTile.UpdateGraphics();
    }
}