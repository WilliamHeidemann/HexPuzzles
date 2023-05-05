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
    BonusSteps
}

public class LevelTile : MonoBehaviour
{
    public TileType tileType = TileType.Empty;
    public int q;
    public int r;
    public int s => -q-r;
    public AxialHex axialHexData;
    private SkinnedMeshRenderer MeshRenderer => GetComponentInChildren<SkinnedMeshRenderer>();
    public Material emptyMaterial;
    public Material standardMaterial;
    public Material blueMaterial;
    public Material teleportMaterial;
    public Material bonusStepMaterial;

    private void OnValidate()
    {
        UpdateGraphics();
    }

    public void UpdateGraphics()
    {
        switch (tileType)
        {
            case TileType.Empty:
                ApplyMaterial(emptyMaterial);
                break;
            case TileType.Standard:
                ApplyMaterial(standardMaterial);
                break;
            case TileType.Blue:
                ApplyMaterial(blueMaterial);
                break;
            case TileType.Teleport:
                ApplyMaterial(teleportMaterial);
                break;
            case TileType.BonusSteps:
                ApplyMaterial(bonusStepMaterial);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ApplyMaterial(Material material)
    {
        MeshRenderer.material = material;
    }

    public void TurnTransparent()
    {
        ApplyMaterial(emptyMaterial);
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
        DestroyImmediate(GetComponent<BlueTile>());
        DestroyImmediate(GetComponent<TeleportTile>());
        DestroyImmediate(GetComponent<BonusStepsTile>());
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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseUpAsButton()
    {
        PlayerMovement.Instance.MoveRequest(this);
    }
}
