using System.Collections;
using ScriptableObjectClasses;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class StarAwardDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite silver;
        [SerializeField] private Sprite gold;
        [SerializeField] private CurrentLevelAsset currentLevel;
        private void OnEnable()
        {
            var starsAwarded = StepCounter.Instance.StarsToAward();
            GetComponent<Image>().sprite = starsAwarded == 3 ? gold : silver;
            UpdateScore(starsAwarded);
            StartCoroutine(AnimateIn());
        }
    
        private void UpdateScore(int starsAwarded)
        {
            var previousBest = PlayerPrefs.GetInt(currentLevel.value.name);
            var best = Mathf.Max(starsAwarded, previousBest);
            PlayerPrefs.SetInt(currentLevel.value.name, best);
        }

        private IEnumerator AnimateIn()
        {
            var time = 0f;
            var rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;
            while (time < 1f)
            {
                var scale = Mathf.SmoothStep(0f, 1f, time);
                rect.localScale = new Vector3(scale, scale, scale);
                time += Time.deltaTime * 3f;
                yield return null;
            }
            rect.localScale = Vector3.one;
        }
    }
}
