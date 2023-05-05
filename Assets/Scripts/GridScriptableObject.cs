using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridScriptableObject : ScriptableObject
{
    public int optimalSteps;
    public List<AxialHex> tileData;
}