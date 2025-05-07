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
    public NavigableOption[] options;
    public float moveSpeed = 10f;
    public Color normalColor = Color.grey;
    public Color highlightColor = Color.white;
    public AudioSource navigationSound;

    public NavigationMode navigationMode = NavigationMode.Vertical;
    public enum NavigationMode
    {
        Vertical,
        Horizontal
    }

    private int currentIndex = 0;

  
    void Start()
    {
        HighlightCurrentOption();
    }

    void Update()
    {
        if (navigationMode == NavigationMode.Vertical)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                Navigate(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                Navigate(1);
        }
        else if (navigationMode == NavigationMode.Horizontal)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Navigate(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                Navigate(1);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayNavigateSound();
            options[currentIndex].onSelected?.Invoke();
        }
    }

    void Navigate(int direction)
    {
        PlayNavigateSound();
        options[currentIndex].Unhighlight(normalColor);
        currentIndex = (currentIndex + direction + options.Length) % options.Length;
        HighlightCurrentOption();
    }

    void HighlightCurrentOption()
    {
        options[currentIndex].Highlight(highlightColor);
    }

    void PlayNavigateSound()
    {
        if ((navigationSound != null))
        {
            navigationSound.pitch = Random.Range(0.9f, 1.1f);
            navigationSound?.Play();
        }
        else
            return;
    }
        
}
