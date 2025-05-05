using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class NavigableOption : MonoBehaviour
{
    public TMP_Text label;
    public UnityEvent onSelected;

    public void Highlight(Color highlightColor) => label.color = highlightColor;
    public void Unhighlight(Color normalColor) => label.color = normalColor;

    private void Start()
    {
        label = GetComponent<TMP_Text>();
    }
}
