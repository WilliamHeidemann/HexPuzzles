using System;
using System.Linq;
using ScriptableObjectClasses;
using TileComponents;
using UnityEngine;

namespace GamePlay
{
    

    public class LevelTile : MonoBehaviour
    {
        public TileType tileType = TileType.Empty;
        public int q;
        public int r;
        public int s => -q-r;
        // public AxialHex axialHexData;
        public bool InRangeOfPlayer { get; set; }
        private SkinnedMeshRenderer MeshRenderer => GetComponentInChildren<SkinnedMeshRenderer>();
        [SerializeField] private Material emptyMaterial;
        [SerializeField] private Material outOfRangeMaterial;
        [SerializeField] private Material standardMaterial;
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material bonusStepMaterial;
        [SerializeField] private Material oneTimeUseMaterial;
        [SerializeField] private Material rotatingMaterial;
        [SerializeField] private Material waterMaterial;
    
        [SerializeField] private GameObject bouquet;
        [SerializeField] private GameObject trampoline;
        [SerializeField] private GameObject moleHill;
        [SerializeField] private GameObject lilyPad;

        [SerializeField] private ParticleSystem availableTileParticleSystem;
        private void OnValidate()
        {
            ShowTile();
        }

        public void UpdateGraphics()
        {
            if (InRangeOfPlayer)
            {
                ShowTile();
            }
            else
            {
                HideTile();
            }
        }

        public void ShowTile()
        {
            UpdateMaterial();
            bouquet.SetActive(tileType == TileType.Blue);
            trampoline.SetActive(tileType == TileType.Jump);
            moleHill.SetActive(tileType == TileType.Teleport);
            lilyPad.SetActive(tileType == TileType.Switch && SwitchTileOn());
        }
    
        private void HideTile()
        {
            MeshRenderer.material = tileType == TileType.Empty ? emptyMaterial : outOfRangeMaterial;
            bouquet.SetActive(false);
            trampoline.SetActive(false);
            moleHill.SetActive(false);
            lilyPad.SetActive(false);
        }
        
        private void UpdateMaterial()
        {
            MeshRenderer.material = tileType switch
            {
                TileType.Empty => emptyMaterial,
                TileType.Standard => standardMaterial,
                TileType.Blue => standardMaterial,
                TileType.Teleport => standardMaterial,
                TileType.BonusSteps => bonusStepMaterial,
                TileType.Jump => emptyMaterial,
                TileType.Switch => waterMaterial,
                TileType.OneTimeUse => oneTimeUseMaterial,
                TileType.Rotating => rotatingMaterial,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void PlayParticles()
        {
            availableTileParticleSystem.Play();
        }
        
        private bool SwitchTileOn()
        {
            var switchTile = (SwitchTile)GetTileComponent(TileType.Switch);
            return switchTile.on;
        }

        public void ApplyTileStruct(AxialHex axialHex)
        {
            // axialHexData = axialHex;
            tileType = axialHex.tileType;
            q = axialHex.Q;
            r = axialHex.R;
            ApplyTileBehaviour(axialHex.tileType);
            if (axialHex.tileType == TileType.Teleport) ConnectTeleportTile(axialHex);
            if (axialHex.tileType == TileType.Switch) GetComponent<SwitchTile>().on = axialHex.switchOn;
            UpdateGraphics();
        }

        private void ConnectTeleportTile(AxialHex axialHex) =>
            GetComponent<TeleportTile>().connectedTile =
                FindObjectsOfType<LevelTile>().First(tile => tile.q == axialHex.teleportQ && tile.r == axialHex.teleportR);

        private void ApplyTileBehaviour(TileType type)
        {
            foreach (var tileComponent in GetComponents<TileComponentBase>())
            {
                DestroyImmediate(tileComponent);
            }
            GetTileComponent(type);
        }

        public TileComponentBase GetTileComponent(TileType type)
        {
            var convertedType = TypeConversion(type);
            if (TryGetComponent(convertedType, out var component))
            {
                return (TileComponentBase)component;
            }
            return (TileComponentBase)gameObject.AddComponent(convertedType);
        }

        private static Type TypeConversion(TileType type)
        {
            return type switch
            {
                TileType.Empty => typeof(UselessTile),
                TileType.Standard => typeof(UselessTile),
                TileType.Blue => typeof(BlueTile),
                TileType.Teleport => typeof(TeleportTile),
                TileType.BonusSteps => typeof(BonusStepsTile),
                TileType.Jump => typeof(JumpTile),
                TileType.Switch => typeof(SwitchTile),
                TileType.OneTimeUse => typeof(OneTimeUseTile),
                TileType.Rotating => typeof(RotatingTile),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private void OnMouseUpAsButton()
        {
            PlayerMovement.Instance.MoveRequest(new MoveCommand(this));
        }
    }
}