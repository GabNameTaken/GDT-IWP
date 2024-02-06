using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    [Header("Boss Icon Settings")]
    [SerializeField] GameObject bossIconGO;

    [Header("Color Settings")]
    [SerializeField] Color activateColor;
    Color originalColor;

    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalColor = image.color;
    }

    public void Activate()
    {
        StartCoroutine(FadeIn());
    }

    public void Completed()
    {
        StartCoroutine(FadeOut());
    }

    float fadeAlpha = 0.15f;
    float fadeDuration = 2.0f;

    IEnumerator FadeOut()
    {
        // Get the initial alpha value of the image
        float startAlpha = image.color.a;

        // Calculate the rate of change per second
        float fadeSpeed = 1.0f / fadeDuration;

        // Loop until the image becomes completely transparent
        while (image.color.a > fadeAlpha)
        {
            // Calculate the new alpha value based on time
            float newAlpha = Mathf.MoveTowards(image.color.a, 0, fadeSpeed * Time.deltaTime);

            // Apply the new color with the updated alpha value
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            // Wait for the next frame
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        // Get the initial alpha value of the image
        image.color = new Color(image.color.r, image.color.g, image.color.b, fadeAlpha);

        // Calculate the rate of change per second
        float fadeSpeed = 1.0f / fadeDuration;

        // Loop until the image becomes completely transparent
        while (image.color.a < 1)
        {
            // Calculate the new alpha value based on time
            float newAlpha = Mathf.MoveTowards(image.color.a, 1, fadeSpeed * Time.deltaTime);

            // Apply the new color with the updated alpha value
            image.color = new Color(activateColor.r, activateColor.g, activateColor.b, newAlpha);

            // Wait for the next frame
            yield return null;
        }
    }
}
