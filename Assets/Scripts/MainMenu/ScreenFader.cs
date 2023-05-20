using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;
    [SerializeField] private Image blackScreen;
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
        var alpha = 1f;
        color.a = alpha;
        blackScreen.color = color;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime;
            color.a = alpha;
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
        var alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            color.a = alpha;
            blackScreen.color = color;
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }
}
