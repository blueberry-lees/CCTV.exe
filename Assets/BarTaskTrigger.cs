using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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

 

    public TextMeshProUGUI targetText;

    public Color normalColor = Color.white;
    public Color highlightedColor = Color.black;

    private List<GameObject> targetPanelInteractables = new();
    public static BarTaskTrigger activeBar; // only one at a time

    private void Awake()
    {
        targetText = GetComponentInChildren<TextMeshProUGUI>();

        // This bar button itself
        targetPanelInteractables.Add(gameObject);

        // All interactables inside this button's panel
        foreach (var sel in targetPanel.GetComponentsInChildren<Selectable>(true))
        {
            targetPanelInteractables.Add(sel.gameObject);
        }

        targetPanel.SetActive(false); // start hidden
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
        targetPanel.gameObject.SetActive(true);

    }


    public void OnPointerExit(PointerEventData eventData)
    {
        selectSound.Play();
        targetText.color = normalColor;
    }


    public void OnSelect(BaseEventData eventData)
    {
        selectSound.Play();
        targetText.color = highlightedColor;

        // Close previous panel if it exists and isn't this
        if (activeBar != null && activeBar != this)
        {
            activeBar.targetPanel.SetActive(false);
        }

        // Set self as current active bar
        activeBar = this;

        // Show this panel
        targetPanel.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectSound.Play();
        targetText.color = normalColor;

        GameObject newSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (newSelected == null || !targetPanelInteractables.Contains(newSelected))
        {
            targetPanel.SetActive(false);

            // If this was the active bar, clear it
            if (activeBar == this)
                activeBar = null;
        }
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
