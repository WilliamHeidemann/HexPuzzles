using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MenuScroller : MonoBehaviour
{
    [SerializeField] private RectTransform worlds;
    [SerializeField] private TextMeshProUGUI worldNumber;
    private static int _worldIndex;
    private int _worldCount;
    
    private float _target;
    private float _start;
    private float _animationTime;

    private void Start()
    {
        _worldCount = worlds.childCount;
        _start = worlds.anchoredPosition.x;
        _target = worlds.anchoredPosition.x;
    }

    private void Update()
    {
        if (_animationTime > 1f) return;
        var position = Mathf.Lerp(_start, _target, _animationTime);
        worlds.anchoredPosition = new Vector2(position, 0f);
        _animationTime += Time.deltaTime * 2f;
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