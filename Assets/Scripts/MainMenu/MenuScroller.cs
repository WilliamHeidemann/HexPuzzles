using ScriptableObjectClasses;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class MenuScroller : MonoBehaviour
    {
        [SerializeField] private RectTransform worlds;
        [SerializeField] private TextMeshProUGUI worldNumber;
        [SerializeField] private CurrentLevelAsset currentLevelAsset;
        [SerializeField] private GameObject scrollLeftButton;
        [SerializeField] private GameObject scrollRightButton;
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
            
            var worldReached = PlayerPrefs.GetInt("World Reached", 0);
            for (int i = 0; i < worldReached; i++)
            {
                Scroll(true);
            }
            scrollLeftButton.SetActive(_worldIndex != 0);
            scrollRightButton.SetActive(_worldIndex != _worldCount - 1);
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
            scrollLeftButton.SetActive(_worldIndex != 0);
            scrollRightButton.SetActive(_worldIndex != _worldCount - 1);
            
            _start = worlds.anchoredPosition.x;
            var target = goingRight ? -worlds.rect.width : worlds.rect.width;
            _target += target;
            _animationTime = 0f;
        }
    }
}