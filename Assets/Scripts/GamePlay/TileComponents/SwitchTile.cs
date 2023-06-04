using GamePlay;

namespace TileComponents
{
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
            GetComponent<LevelTile>().ShowTile();
        }

        public void Trigger()
        {
            var levelTile = GetComponent<LevelTile>();
            if (PlayerMovement.Instance.current == levelTile) return;
            on = !on;
            levelTile.UpdateGraphics();
        }
    }
}