using System;
using System.Collections;
using System.Collections.Generic;
using MainMenu;
using UnityEngine;

public class ManualScrollDetector : MonoBehaviour
{
    private const float MinSwipeDistance = 60f;
    private Vector2 _startPos;
    private MenuScroller _menuScroller;

    private void Start() => _menuScroller = GetComponent<MenuScroller>();

    void Update()
    {
        if (Input.touchCount <= 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began) _startPos = touch.position;
        if (touch.phase == TouchPhase.Ended)
        {
            var swipeDelta = touch.position - _startPos;
            if (!(swipeDelta.magnitude > MinSwipeDistance)) return;
            var swipeAngle = Mathf.Atan2(swipeDelta.y, swipeDelta.x) * Mathf.Rad2Deg;
            if (swipeAngle < 0) swipeAngle += 360;
            if (swipeAngle is < 45 or > 315)
            {
                // Swipe right
                print(swipeDelta.magnitude);
                _menuScroller.Scroll(false);
            }
            else if (swipeAngle is > 135 and < 225)
            {
                // Swipe left
                _menuScroller.Scroll(true);
            }
        }
    }
}
