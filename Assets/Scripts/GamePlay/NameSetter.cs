using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSetter : MonoBehaviour
{
    private TextMeshProUGUI _name;
    private void Awake()
    {
        _name = GetComponent<TextMeshProUGUI>();
        LevelLoader.EnterLevelEvent += SetLevelName;
    }
    private void SetLevelName(GridScriptableObject level) => _name.text = level.name;
}
