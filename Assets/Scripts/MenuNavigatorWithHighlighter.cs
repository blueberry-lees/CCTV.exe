using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Must go with "NavigableOption" script
/// This script is used to manage panels with choices, attach this script to a non-option obj/ the panel that holds the options.
/// And then attach the "NavigableOption" script on the selectable objs with text ui.And drag them into the 'NavigableOption' list. 
/// this script also includes a highlighter box, make sure the box is under the same canvas.
/// Note: this script has only been tested on obj under Canvas
/// </summary>
public class MenuNavigatorWithHighlighter : MonoBehaviour
{
    [Header("References")]
    public RectTransform highlightBox;
    public AudioSource typeSound;

    [Header("Navigation Settings")]
    public float moveSpeed = 10f;
    public float scaleUp = 1.1f;
    public float scaleSpeed = 10f;
    public Color normalColor = Color.grey;
    public Color highlightColor = Color.white;

    [Header("Options")]
    public NavigableOption[] options;
    private List<NavigableOption> filteredOptions = new List<NavigableOption>();

    private int currentIndex = 0;

    void Start()
    {
        // Filter null or inactive options
        foreach (var option in options)
        {
            if (option != null && option.gameObject.activeInHierarchy)
                filteredOptions.Add(option);
        }

        StartCoroutine(InitHighlightPosition());
    }

    IEnumerator InitHighlightPosition()
    {
        yield return new WaitForEndOfFrame();
        foreach (var group in GetComponentsInChildren<LayoutGroup>())
            LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());

        if (filteredOptions.Count > 0)
            MoveHighlightTo(currentIndex, true);
    }

    void Update()
    {
        if (filteredOptions.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Navigate(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            Navigate(1);
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayNavigateSound();
            filteredOptions[currentIndex].onSelected?.Invoke();
        }
    }

    void Navigate(int direction)
    {
        PlayNavigateSound();
        filteredOptions[currentIndex].Unhighlight(normalColor);
        currentIndex = (currentIndex + direction + filteredOptions.Count) % filteredOptions.Count;
        MoveHighlightTo(currentIndex);
    }

    void MoveHighlightTo(int index, bool instant = false)
    {
        StopAllCoroutines();

        for (int i = 0; i < filteredOptions.Count; i++)
        {
            if (i == index) continue;
            filteredOptions[i].Unhighlight(normalColor);
        }

        if (instant)
        {
            highlightBox.position = filteredOptions[index].transform.position;
        }
        else
        {
            StartCoroutine(SmoothMove(filteredOptions[index].transform as RectTransform));
            StartCoroutine(ScaleHighlight());
        }

        filteredOptions[index].Highlight(highlightColor);
    }

    IEnumerator SmoothMove(RectTransform target)
    {
        while (Vector3.Distance(highlightBox.position, target.position) > 0.01f)
        {
            highlightBox.position = Vector3.Lerp(highlightBox.position, target.position, Time.deltaTime * moveSpeed);
            yield return null;
        }
        highlightBox.position = target.position;
    }

    IEnumerator ScaleHighlight()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = Vector3.one * scaleUp;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            highlightBox.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            highlightBox.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        highlightBox.localScale = originalScale;
    }

    void PlayNavigateSound()
    {
        if (typeSound != null)
        {
            typeSound.pitch = Random.Range(0.9f, 1.1f);
            typeSound.Play();
        }
    }
}
