﻿using GamePlay;
using UnityEngine;

namespace TileComponents
{
    public class TeleportTile : TileComponentBase, IActivatedTile
    {
        public LevelTile connectedTile;
        public void Activate()
        {
            // GetComponent<AudioSource>().Play();
            PlayerMovement.Instance.MoveRequest(new MoveCommand(connectedTile, shouldTriggerTileEvent: false, shouldActivateTile: false, shouldIncrementStepCount: false, shouldCheckRange:false, shouldDisplayAllTiles: true));
        }

        private void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, connectedTile.transform.position, Color.black);
        }
    }
}