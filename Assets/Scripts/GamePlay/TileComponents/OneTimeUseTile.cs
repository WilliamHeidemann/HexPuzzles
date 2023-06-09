﻿using GamePlay;
using ScriptableObjectClasses;

namespace TileComponents
{
    public class OneTimeUseTile : TileComponentBase, IEventTriggerTile, IActivatedTile
    {
        private bool _steppedOn;
    
        private void Awake()
        {
            PlayerMovement.TriggerTileEvent += Trigger;
        }

        private void OnDestroy()
        {
            PlayerMovement.TriggerTileEvent -= Trigger;
        }

        public void Activate()
        {
            _steppedOn = true;
        }
    
        public void Trigger()
        {
            if (!_steppedOn) return;
            var levelTile = GetComponent<LevelTile>();
            levelTile.tileType = TileType.Empty;
            levelTile.UpdateGraphics();
            Destroy(this);
        }
    }
}
