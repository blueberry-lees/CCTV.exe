using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class BarTaskTrigger : 
    MonoBehaviour,
    IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler,
        IUpdateSelectedHandler,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        ICancelHandler
{

    public AudioSource selectSound;
    public GameObject targetPanel;
    public CanvasGroup barGroup;
    public CanvasGroup previewGroup;

    public TextMeshProUGUI targetText;

    public Color normalColor = Color.white;
    public Color highlightedColor = Color.black;

    public void Awake()
    {
        targetText = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectSound.Play();
        targetText.color = highlightedColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectSound.Play();
        targetText.color = highlightedColor;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        targetText.color = normalColor;
    }


    public void OnSelect(BaseEventData eventData)
    {
        targetText.color = highlightedColor;
        targetPanel.gameObject.SetActive(true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        targetText.color = normalColor;
        targetPanel.gameObject.SetActive(false);


    }



    //Called every frame while this object is selected (useful for holding keys while selected).
    public void OnUpdateSelected(BaseEventData eventData)
    {
        targetText.color = highlightedColor;
    }


    public void OnSubmit(BaseEventData eventData)
    {
        selectSound.Play();
        targetText.color = highlightedColor;

    }
    public void OnCancel(BaseEventData eventData)
    {
        selectSound.Play();
        targetText.color = highlightedColor;
    }








}
