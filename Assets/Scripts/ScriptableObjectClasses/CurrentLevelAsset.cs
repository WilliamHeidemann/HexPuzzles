using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Current Level Data")]
public class CurrentLevelAsset : ScriptableObject
{
    public GridScriptableObject value;
    public World world;
}
