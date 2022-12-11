using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SceneFade : MonoBehaviour
{
    private Image fadeColor;
    private float smoothessValue = 0.01f;

    private void Start()
    {
        fadeColor = GetComponent<Image>();
        StartCoroutine(StartFade());
    }

    private IEnumerator StartFade()
    {
        while (fadeColor.color.a >= 0.01f)
        {
            fadeColor.color = new Color(fadeColor.color.r, fadeColor.color.g, fadeColor.color.b, Mathf.Lerp(fadeColor.color.a, 0f, smoothessValue));
            yield return new WaitForEndOfFrame();
        }

        fadeColor.gameObject.SetActive(false);

        yield return null;
    }
}
