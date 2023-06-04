using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class ScreenFader : MonoBehaviour
    {
        public static ScreenFader instance;
        [SerializeField] private Image blackScreen;
        private const float FadeTimeInSeconds = .2f;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var color = blackScreen.color;
            color.a = 1f;
            blackScreen.color = color;
            var time = 0f;
            while (time < FadeTimeInSeconds)
            {
                time += Time.deltaTime;
                color.a = 1f - time / FadeTimeInSeconds;
                blackScreen.color = color;
                yield return null;
            }
        }

        public void FadeTo(string scene)
        {
            blackScreen.raycastTarget = true;
            StartCoroutine(FadeOut(scene));
        }

        private IEnumerator FadeOut(string scene)
        {
            var color = blackScreen.color;
            var time = 0f;
            while (time < FadeTimeInSeconds)
            {
                time += Time.deltaTime;
                color.a = time / FadeTimeInSeconds;
                blackScreen.color = color;
                yield return null;
            }

            // var sceneAsync = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            // yield return new WaitUntil(() => sceneAsync.isDone);
            // SceneManager.UnloadSceneAsync(0);
            SceneManager.LoadScene(scene);
        }
    }
}
