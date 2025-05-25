using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class BlinkTMPText : MonoBehaviour
{
    public float blinkInterval = 0.5f; // Time between blinks

    private TMP_Text tmpText;
    private Coroutine blinkRoutine;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        blinkRoutine = StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        if (tmpText != null)
            tmpText.enabled = true; // Ensure visible when disabled
    }

    IEnumerator Blink()
    {
        while (true)
        {
            tmpText.enabled = !tmpText.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
