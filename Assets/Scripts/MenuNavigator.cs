using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuNavigator : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Color normalColor = Color.grey;
    public Color highlightColor = Color.white;
    public AudioSource typeSound;

    public NavigableOption[] options;
    private int currentIndex = 0;

    void Start()
    {
        HighlightCurrentOption();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
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
        typeSound.pitch = Random.Range(0.9f, 1.1f);
        typeSound?.Play();
    }
}
