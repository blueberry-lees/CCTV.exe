using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvertTextColorOnSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IUpdateSelectedHandler
{
    public TextMeshProUGUI targetText;

    public Color normalColor = Color.white;
    public Color highlightedColor = Color.black;

    public void OnPointerEnter(PointerEventData eventData) => targetText.color = highlightedColor;
    public void OnPointerExit(PointerEventData eventData) => targetText.color = normalColor;

    public void OnSelect(BaseEventData eventData) => targetText.color = highlightedColor;
    public void OnDeselect(BaseEventData eventData) => targetText.color = normalColor;

    public void OnUpdateSelected(BaseEventData eventData)
    {
        targetText.color = highlightedColor;
    }

    public void ChangeTextToBlack()
    {
        targetText.color = highlightedColor;
    }
    public void ChangeTextToWhite()
    {
        targetText.color = normalColor;
    }
}