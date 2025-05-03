using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlicker2 : MonoBehaviour
{
    public TMP_Text flickerText;
    public float flickerSpeed = 0.1f;  // Flicker every 0.1 seconds

    private bool isFlickering = false;

    public void StartFlicker()
    {
        if (!isFlickering)
            StartCoroutine(FlickerRoutine());
    }

    public void StopFlicker()
    {
        isFlickering = false;
        flickerText.alpha = 1f; // Reset to visible
    }

    private System.Collections.IEnumerator FlickerRoutine()
    {
        isFlickering = true;

        while (isFlickering)
        {
            flickerText.alpha = Random.value > 0.5f ? 1f : 0f;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
}

