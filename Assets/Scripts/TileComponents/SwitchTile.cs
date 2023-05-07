using System;
using UnityEngine;

public class SwitchTile : MonoBehaviour, IEventTriggerTile
{
    private LevelTile _levelTile;
    
    private void Awake()
    {
        PlayerMovement.PlayerMoved += Trigger;
    }

    private void Start()
    {
        _levelTile = GetComponent<LevelTile>();
    }

    private void OnDestroy()
    {
        PlayerMovement.PlayerMoved -= Trigger;
    }

    public void Trigger()
    {
        if (PlayerMovement.Instance.current == _levelTile) return;
        
    }
}