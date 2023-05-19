using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World", fileName = "New World")]
public class World : ScriptableObject
{
    public GridScriptableObject centerLevel;
    public GridScriptableObject[] connectedLevels;
}