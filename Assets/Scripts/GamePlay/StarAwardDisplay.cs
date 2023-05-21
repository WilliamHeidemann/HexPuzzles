using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarAwardDisplay : MonoBehaviour
{
    [SerializeField] private Sprite silver;
    [SerializeField] private Sprite gold;
    private void OnEnable()
    {
        var starsAwarded = StepCounter.Instance.StarsToAward();
        GetComponent<Image>().sprite = starsAwarded == 3 ? gold : silver;
        StartCoroutine(AnimateIn());
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
