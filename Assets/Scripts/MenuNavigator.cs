using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuNavigator : MonoBehaviour
{
    public RectTransform highlightBox;
    public float moveSpeed = 10f;
    public float scaleUp = 1.1f;
    public float scaleSpeed = 10f;
    public Button[] optionButtons;
    private int currentIndex = 0;

    void Start()
    {
        StartCoroutine(InitHighlightPosition());
    }

    IEnumerator InitHighlightPosition()
    {
        yield return new WaitForEndOfFrame(); // Wait one frame for layout to settle
        MoveHighlightTo(optionButtons[currentIndex].transform as RectTransform, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex = (currentIndex - 1 + optionButtons.Length) % optionButtons.Length;
            MoveHighlightTo(optionButtons[currentIndex].transform as RectTransform);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex = (currentIndex + 1) % optionButtons.Length;
            MoveHighlightTo(optionButtons[currentIndex].transform as RectTransform);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            optionButtons[currentIndex].onClick.Invoke();
        }
    }

    void MoveHighlightTo(RectTransform target, bool instant = false)
    {
        StopAllCoroutines();
        if (instant)
        {
            highlightBox.position = target.position;
        }
        else
        {
            StartCoroutine(SmoothMove(target));
            StartCoroutine(ScaleHighlight());
        }
    }

    System.Collections.IEnumerator SmoothMove(RectTransform target)
    {
        while (Vector3.Distance(highlightBox.position, target.position) > 0.01f)
        {
            highlightBox.position = Vector3.Lerp(highlightBox.position, target.position, Time.deltaTime * moveSpeed);
            yield return null;
        }
        highlightBox.position = target.position;
    }

    System.Collections.IEnumerator ScaleHighlight()
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
}
