using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class NavigableOption : MonoBehaviour
{
    [Tooltip("The text label to change color on highlight. Will auto-assign if left empty.")]
    public TMP_Text label;
    public UnityEvent onSelected;

    public void Highlight(Color highlightColor)
    {
        if (label != null)
            label.color = highlightColor;
    }

    public void Unhighlight(Color normalColor)
    {
        if (label != null)
            label.color = normalColor;
    }

    private void Awake()
    {
        // Only assign if not set in the Inspector
        if (label == null)
            label = GetComponent<TMP_Text>();
    }
}
