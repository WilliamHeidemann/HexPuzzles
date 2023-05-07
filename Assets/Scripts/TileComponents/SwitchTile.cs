﻿using System;
using UnityEngine;

public class SwitchTile : TileComponentBase, IEventTriggerTile
{
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

    public void Trigger()
    {
        var levelTile = GetComponent<LevelTile>();
        if (PlayerMovement.Instance.current == levelTile) return;
        on = !on;
        levelTile.UpdateGraphics();
    }
}