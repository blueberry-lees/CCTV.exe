using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Must go with "NavigableOption" script
/// This script is used to manage panels with choices, attach this script to a non-option obj/ the panel that holds the options.
/// And then attach the "NavigableOption" script on the selectable objs with text ui.And drag them into the 'NavigableOption' list. 
/// Note: this script has only been tested on obj under Canvas
/// </summary>
public class MenuNavigator : MonoBehaviour
{
    [Header("Settings")]
    public NavigableOption[] options;  // Will filter at runtime
    private List<NavigableOption> filteredOptions = new List<NavigableOption>();

    public float moveSpeed = 10f;
    public Color normalColor = Color.grey;
    public Color highlightColor = Color.white;
    public AudioSource navigationSound;

    public NavigationMode navigationMode = NavigationMode.Vertical;
    public enum NavigationMode { Vertical, Horizontal }

    private int currentIndex = 0;

    void Start()
    {
        // Remove null or inactive options at runtime
        foreach (var option in options)
        {
            if (option != null && option.gameObject.activeInHierarchy)
                filteredOptions.Add(option);
        }

        StartCoroutine(DelayedHighlight());
    }

    IEnumerator DelayedHighlight()
    {
        yield return null; // Wait one frame

        foreach (var group in GetComponentsInChildren<LayoutGroup>())
            LayoutRebuilder.ForceRebuildLayoutImmediate(group.GetComponent<RectTransform>());

        if (filteredOptions.Count > 0)
            HighlightCurrentOption();
    }

    void Update()
    {
        if (filteredOptions.Count == 0) return;

        if (navigationMode == NavigationMode.Vertical)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) Navigate(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) Navigate(1);
        }
        else if (navigationMode == NavigationMode.Horizontal)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) Navigate(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow)) Navigate(1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayNavigateSound();
            filteredOptions[currentIndex].onSelected?.Invoke();
        }
    }

    void Navigate(int direction)
    {
        PlayNavigateSound();
        if (currentIndex < filteredOptions.Count)
            filteredOptions[currentIndex].Unhighlight(normalColor); currentIndex = (currentIndex + direction + filteredOptions.Count) % filteredOptions.Count;
        HighlightCurrentOption();
    }

    void HighlightCurrentOption()
    {
        filteredOptions[currentIndex].Highlight(highlightColor);
    }

    void PlayNavigateSound()
    {
        if (navigationSound != null)
        {
            navigationSound.pitch = Random.Range(0.9f, 1.1f);
            navigationSound.Play();
        }
    }




}
