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
    Jump
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
            _ => throw new ArgumentOutOfRangeException()
        };
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
        UpdateGraphics();
    }

    private void ConnectTeleportTile(AxialHex axialHex) =>
        GetComponent<TeleportTile>().connectedTile =
            FindObjectsOfType<LevelTile>().First(tile => tile.q == axialHex.teleportQ && tile.r == axialHex.teleportR);

    private void ApplyTileBehaviour(TileType type)
    {
        foreach (var activatedTile in GetComponents<IActivatedTile>())
        {
            DestroyImmediate((Component)activatedTile);
        }
        
        switch (type)
        {
            case TileType.Empty:
                break;
            case TileType.Standard:
                break;
            case TileType.Blue:
                if (GetComponent<BlueTile>() == null) gameObject.AddComponent<BlueTile>();
                break;
            case TileType.Teleport:
                if (GetComponent<TeleportTile>() == null) gameObject.AddComponent<TeleportTile>();
                break;
            case TileType.BonusSteps:
                if (GetComponent<BonusStepsTile>() == null) gameObject.AddComponent<BonusStepsTile>();
                break;
            case TileType.Jump:
                if (GetComponent<JumpTile>() == null) gameObject.AddComponent<JumpTile>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseUpAsButton()
    {
        PlayerMovement.Instance.MoveRequest(this);
    }
}
