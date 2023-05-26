using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectClasses
{
    public class GridScriptableObject : ScriptableObject
    {
        public int optimalSteps;
        public List<AxialHex> tileData;
        public bool updatingAllowed;
        public bool LevelIsComplete => PlayerPrefs.GetInt(name) > 0;
    }
}