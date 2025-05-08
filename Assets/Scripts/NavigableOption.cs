using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Must go with "NavigableOption" script
/// Attach the "NavigableOption" script on the selectable objs with text ui.And drag them into the 'NavigableOption' list in "NavigableOption" script. 
/// Note: this script has only been tested on obj under Canvas
/// </summary>
public class NavigableOption : MonoBehaviour
{
    [Tooltip("The text label to change color on highlight. Will auto-assign if left empty.")]
    public TMP_Text label;
    public UnityEvent onSelected;

    [Header("Shake Settings")]
    public float shakeIntensity = 0.5f; // Default shake intensity
    public bool enableShakeOnSelect = false; // Enable or disable shake on selection
    public float onSelectShakeIntensity = 1f; // Shake intensity when selected

    private Coroutine shakeCoroutine;
    private Vector3 originalPos;

    private RectTransform rectTransform;
    private Vector2 originalAnchoredPos;

    private void Awake()
    {
        if (label == null)
            label = GetComponent<TMP_Text>();

        rectTransform = GetComponent<RectTransform>();
        
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedShakeInit());
    }

    private IEnumerator DelayedShakeInit()
    {
        yield return null; // Wait one frame
        rectTransform = GetComponent<RectTransform>();
        originalAnchoredPos = rectTransform.anchoredPosition;

        if (!enableShakeOnSelect)
            shakeCoroutine = StartCoroutine(ShakeRoutine(shakeIntensity));
    }
    private void OnDisable()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            rectTransform.anchoredPosition = originalAnchoredPos;
        }
    }

    public void Highlight(Color highlightColor)
    {
        if (label != null)
            label.color = highlightColor;

        // If enabled, shake with the selection intensity
        if (enableShakeOnSelect)
            RestartShake(onSelectShakeIntensity);
        else
            RestartShake(shakeIntensity);
    }

    public void Unhighlight(Color normalColor)
    {
        if (label != null)
            label.color = normalColor;

        // If enabled, shake with the default intensity
        if (enableShakeOnSelect)
            RestartShake(shakeIntensity);
        else
            RestartShake(shakeIntensity); // Maintain the default shake intensity
    }

    private void RestartShake(float intensity)
    {
        // Stop any previous shake coroutine and reset position
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        rectTransform.anchoredPosition = originalAnchoredPos;
        shakeCoroutine = StartCoroutine(ShakeRoutine(intensity)); 
    }

    private IEnumerator ShakeRoutine(float intensity)
    {
        while (true)
        {
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);
            rectTransform.anchoredPosition = originalAnchoredPos + new Vector2(offsetX, offsetY);

            yield return new WaitForSeconds(0.02f);
        }
    }
}
