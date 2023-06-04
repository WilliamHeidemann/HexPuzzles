using UnityEngine;

namespace MainMenu
{
    public class ManualScrollDetector : MonoBehaviour
    {
        private const float MinSwipeDistance = 100f;
        private float _startX;
        private MenuScroller _menuScroller;

        private void Start() => _menuScroller = GetComponent<MenuScroller>();

        private void Update()
        {
            if (Input.touchCount <= 0) return;
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) _startX = touch.position.x;
            if (touch.phase == TouchPhase.Ended)
            {
                var swipeDelta = touch.position.x - _startX;
                if (!(Mathf.Abs(swipeDelta) > MinSwipeDistance)) return;
                _menuScroller.Scroll(!(swipeDelta > 0));
            }
        }
    }
}
