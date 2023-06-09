using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectClasses;
using TileComponents;
using UnityEngine;

namespace GamePlay
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private AudioSource hopSound;
        public static PlayerMovement Instance;
        public static event Action TriggerTileEvent;
        private static IEnumerable<LevelTile> LevelTiles => FindObjectsOfType<LevelTile>();
        private const int ViewDistance = 5;
        [HideInInspector] public LevelTile previous;
        [HideInInspector] public LevelTile current;

        private void Awake()
        {
            if (Instance != null) Destroy(this);
            Instance = this;
            LevelLoader.EnterLevelEvent += CenterPlayer;
        }

        private void OnDestroy()
        {
            LevelLoader.EnterLevelEvent -= CenterPlayer;
        }

        private void CenterPlayer(GridScriptableObject level)
        {
            previous = null;
            current = LevelTiles.First(tile => tile.q == 0 && tile.r == 0);
            DisplayTilesInRange(current);
            transform.position = current.transform.position + new Vector3(0f, 0.25f, 0f);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    
        public void MoveRequest(MoveCommand command)
        {
            if (IsInvalid(command))
            {
                ShowWherePlayerCanGo();
                return;
            }
            hopSound.Play();
            previous = current;
            current = command.Tile;
            DisplayTilesInRange(current);
            PlayerMovementAnimation.instance.MoveTo(command);
        }

        public static void MoveRequestCompleted(MoveCommand command)
        {
            if (command.ShouldTriggerTileEvent) TriggerTileEvent?.Invoke();
            if (command.ShouldActivateTile)
                if (command.Tile.TryGetComponent<IActivatedTile>(out var activatedTile)) 
                    activatedTile.Activate();
            if (command.ShouldIncrementStepCount) StepCounter.Instance.IncrementStepCount();
            ObjectiveManager.instance.ProgressionCheck();
        }

        private static bool IsInvalid(MoveCommand command)
        {
            if (StepCounter.Instance.IsOutOfSteps) return true;
            if (ObjectiveManager.instance.LevelComplete) return true;
            if (command.Tile.tileType == TileType.Empty) return true;
            if (command.Tile.tileType == TileType.Switch && !command.Tile.GetComponent<SwitchTile>().on) return true;
            if (command.Tile == Instance.current) return true;
            if (command.ShouldCheckRange)
                if (!InRange(Instance.current, command.Tile, 1)) return true;
            return false;
        }
        
        private static void ShowWherePlayerCanGo()
        {
            foreach (var tile in LevelTiles)
            {
                if (!InRange(Instance.current, tile, 1)) continue;
                if (tile.tileType == TileType.Empty) continue;
                if (tile.tileType == TileType.Switch && !tile.GetComponent<SwitchTile>().on) continue;
                if (tile == Instance.current) continue;
                tile.PlayParticles();
            }
        }

        private static void DisplayAllTiles()
        {
            foreach (var tile in LevelTiles)
            {
                tile.InRangeOfPlayer = true;
                tile.UpdateGraphics();
            }
        }
    
        private static void DisplayTilesInRange(LevelTile aroundTile)
        {
            foreach (var tile in LevelTiles)
            {
                tile.InRangeOfPlayer = InRange(aroundTile, tile, ViewDistance);
                tile.UpdateGraphics(); // If it's already in range, maybe dont update graphics again
            }
        }
    
        private static bool InRange(LevelTile first, LevelTile second, int range)
        {
            if (first == null || second == null) return false;
            var qDiff = Mathf.Abs(first.q - second.q);
            var rDiff = Mathf.Abs(first.r - second.r);
            var sDiff = Mathf.Abs(first.s - second.s);
            var distance = Mathf.Max(qDiff, rDiff, sDiff);
            return distance <= range;
        }
    }
}
