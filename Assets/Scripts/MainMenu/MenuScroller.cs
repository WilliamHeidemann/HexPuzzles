using ScriptableObjectClasses;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuScroller : MonoBehaviour
    {
        [SerializeField] private RectTransform worlds;
        [SerializeField] private TextMeshProUGUI worldNumber;
        [SerializeField] private CurrentLevelAsset currentLevelAsset;
        [SerializeField] private Image scrollLeftImage;
        [SerializeField] private Image scrollRightImage;
        [SerializeField] private Sprite scrollLockedSprite;
        [SerializeField] private Sprite scrollArrowSprite;
        private int _worldReached;
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
            
            _worldReached = PlayerPrefs.GetInt("World Reached", 0);
            for (int i = 0; i < _worldReached; i++)
            {
                Scroll(true);
            }

            CorrectButtonImages();
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
            // if (goingRight && _worldIndex == _worldCount - 1) return;
            if (goingRight && _worldIndex + 1 > _worldReached) return;
            if (!goingRight && _worldIndex == 0) return;
            _worldIndex += goingRight ? 1 : -1;
            worldNumber.text = (_worldIndex + 1).ToString();
            CorrectButtonImages();
            
            _start = worlds.anchoredPosition.x;
            var target = goingRight ? -worlds.rect.width : worlds.rect.width;
            _target += target;
            _animationTime = 0f;
        }

        private void CorrectButtonImages()
        {
            scrollLeftImage.gameObject.SetActive(_worldIndex != 0);
            scrollRightImage.gameObject.SetActive(_worldIndex != _worldCount - 1);
            scrollRightImage.sprite = _worldIndex + 1 <= _worldReached  ? scrollArrowSprite : scrollLockedSprite;
        }
    }
}