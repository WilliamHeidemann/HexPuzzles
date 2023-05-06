using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Order")]
public class LevelOrder : ScriptableObject
{
    public List<GridScriptableObject> orderedLevels;
}