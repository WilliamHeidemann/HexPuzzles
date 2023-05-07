using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum TileType
{
    Empty,
    Standard,
    Blue,
    Teleport,
    BonusSteps,
    Jump,
    Switch
}

public class LevelTile : MonoBehaviour
{
    public TileType tileType = TileType.Empty;
    public int q;
    public int r;
    public int s => -q-r;
    public AxialHex axialHexData;
    private SkinnedMeshRenderer MeshRenderer => GetComponentInChildren<SkinnedMeshRenderer>();
    [SerializeField] private Material emptyMaterial;
    [SerializeField] private Material standardMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material teleportMaterial;
    [SerializeField] private Material bonusStepMaterial;
    [SerializeField] private Material jumpMaterial;
    [SerializeField] private Material switchOnMaterial;
    [SerializeField] private Material switchOffMaterial;

    private void OnValidate()
    {
        UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        MeshRenderer.material = tileType switch
        {
            TileType.Empty => emptyMaterial,
            TileType.Standard => standardMaterial,
            TileType.Blue => blueMaterial,
            TileType.Teleport => teleportMaterial,
            TileType.BonusSteps => bonusStepMaterial,
            TileType.Jump => jumpMaterial,
            TileType.Switch => SwitchMaterial(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Material SwitchMaterial()
    {
        var switchTile = (SwitchTile)GetTileComponent(TileType.Switch);
        return switchTile.on ? switchOnMaterial : switchOffMaterial;
    }

    public void TurnTransparent()
    {
        MeshRenderer.material = emptyMaterial;
    }

    public void ApplyTileStruct(AxialHex axialHex)
    {
        axialHexData = axialHex;
        tileType = axialHex.tileType;
        q = axialHex.Q;
        r = axialHex.R;
        ApplyTileBehaviour(axialHex.tileType);
        if (axialHex.tileType == TileType.Teleport) ConnectTeleportTile(axialHex);
        if (axialHex.tileType == TileType.Switch)
        {
            var switchTile = (SwitchTile)GetTileComponent(TileType.Switch);
            switchTile.on = axialHex.switchOn;
        }
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
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private void OnMouseUpAsButton()
    {
        PlayerMovement.Instance.MoveRequest(this);
    }
}
