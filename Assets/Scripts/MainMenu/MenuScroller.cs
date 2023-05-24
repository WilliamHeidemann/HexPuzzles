using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MenuScroller : MonoBehaviour
{
    [SerializeField] private RectTransform worlds;
    [SerializeField] private TextMeshProUGUI worldNumber;
    [SerializeField] private CurrentLevelAsset currentLevelAsset;
    private int _worldIndex;
    private int _worldCount;
    
    private float _target;
    private float _start;
    private float _animationTime;
    
    private void Start()
    {
        _worldCount = worlds.childCount;
        _start = worlds.anchoredPosition.x * currentLevelAsset.world.index;
        _target = _start;

        for (int i = 0; i < currentLevelAsset.world.index + 1; i++)
        {
            Scroll(true);
        }
    }
    
    private void Update()
    {
        if (_animationTime > 1f) return;
        _animationTime += Time.deltaTime * 2f;
        var position = Mathf.Lerp(_start, _target, _animationTime);
        worlds.anchoredPosition = new Vector2(position, 0f);
    }

    public void Scroll(bool goingRight)
    {
        if (goingRight && _worldIndex == _worldCount - 1) return;
        if (!goingRight && _worldIndex == 0) return;
        _worldIndex += goingRight ? 1 : -1;
        worldNumber.text = (_worldIndex + 1).ToString();
        
        _start = worlds.anchoredPosition.x;
        var target = goingRight ? -worlds.rect.width : worlds.rect.width;
        _target += target;
        _animationTime = 0f;
    }
}