using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LineTypewriter : MonoBehaviour
{

    private TextMeshProUGUI targetText;
    private string[] lines;
    private string originalText;
    public float delayBetweenLines = .03f;


    private void Start()
    {
        targetText = GetComponent<TextMeshProUGUI>();
        if (targetText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on this GameObject.");
            enabled = false;
            return;
        }

        originalText = targetText.text; // Cache the original text
        lines = originalText.Split(new[] { '\n' }, System.StringSplitOptions.None);

        StartCoroutine(TypeLines());
    }

    private void OnEnable()
    {
        // If Start hasn't run yet, skip to avoid null refs
        if (lines != null && targetText != null)
            StartCoroutine(TypeLines());
    }

    IEnumerator TypeLines()
    {
        targetText.text = ""; // clear before typing

        foreach (string line in lines)
        {
            targetText.text += line + "\n";
            yield return new WaitForSeconds(delayBetweenLines);
        }
    }
}