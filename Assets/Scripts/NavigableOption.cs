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

    private void Awake()
    {
        if (label == null)
            label = GetComponent<TMP_Text>();

        originalPos = transform.localPosition;
    }

    private void OnEnable()
    {
        // Start shaking with the default shake intensity if enabled
        if (!enableShakeOnSelect)
            shakeCoroutine = StartCoroutine(ShakeRoutine(shakeIntensity));
    }

    private void OnDisable()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            transform.localPosition = originalPos;
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

        transform.localPosition = originalPos; // Reset position to avoid stuck offsets
        shakeCoroutine = StartCoroutine(ShakeRoutine(intensity));
    }

    private IEnumerator ShakeRoutine(float intensity)
    {
        // Continuously shake the object within the given intensity range
        while (true)
        {
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            yield return new WaitForSeconds(0.02f); // Controls shake speed
        }
    }
}
