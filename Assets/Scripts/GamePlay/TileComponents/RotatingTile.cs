using System;
using System.Linq;
using GamePlay;
using UnityEngine;

namespace TileComponents
{
    internal enum TileDirection
    {
        Right,
        LowerRight,
        LowerLeft,
        Left,
        UpperLeft,
        UpperRight
    }

    public class RotatingTile : TileComponentBase, IActivatedTile, IEventTriggerTile
    {
        [SerializeField] private TileDirection startDirection;
        [SerializeField] [Range(1,3)] private int rotationsPerMove = 1;
        [SerializeField] private bool counterClockwiseRotation;
        private readonly int[] _qr = { -1, 0, 1, 1, 0, -1};
        private int _qIndex = 0;
        private int _rIndex = 4;
        private int Rotation => counterClockwiseRotation ? rotationsPerMove : -rotationsPerMove;
        private LevelTile LevelTile => GetComponent<LevelTile>();
        private int TargetQ => LevelTile.q + _qr[_qIndex];
        private int TargetR => LevelTile.r + _qr[_rIndex];
        private LevelTile TilePointedTo =>
            FindObjectsOfType<LevelTile>().First(tile => tile.q == TargetQ && tile.r == TargetR);

        public Transform sphere;
    
        private void Awake()
        {
            PlayerMovement.TriggerTileEvent += Trigger;
        }

        private void Start()
        {
            CreateSphere();
        }

        private void CreateSphere()
        {
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            sphere.parent = transform;
            PositionSphere();
        }

        private void OnDestroy()
        {
            PlayerMovement.TriggerTileEvent -= Trigger;
            Destroy(sphere.gameObject);
        }

        private void RotationLogic()
        {
            _qIndex = (_qIndex + Rotation + 6) % 6;
            _rIndex = (_rIndex + Rotation + 6) % 6;
        }

        public void Trigger()
        {
            if (PlayerMovement.Instance.current == LevelTile) return;
            RotationLogic();
            PositionSphere();
            // Play animation
        }
    
        private void PositionSphere() => sphere.position = Vector3.Lerp(transform.position, TilePointedTo.transform.position, 0.3f);

        // Trigger happens before activate, so the player will walk into the tile, the tile will then rotate to a new place, 
        // and send the player there instead of where they intended when the first clicked the tile. 
    
        public void Activate()
        {
            PlayerMovement.Instance.MoveRequest(new MoveCommand(TilePointedTo, shouldIncrementStepCount: false));
        }
    }

// q, r = 0, 0 => middle
// -1, 0 => left
// 0, -1 => upper left
// +1, -1 => upper right
// +1, 0 => right
// 0, +1 => lower right
// -1, +1 => lower left
}