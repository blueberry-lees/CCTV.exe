using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class NameUIController : MonoBehaviour
{
  
    public GameObject nameInputPanel;
    public TMP_Text namePromptText; // Reference to "What's your name?"
    public TMP_InputField nameInputField;
    public TextMeshProUGUI introText;
    public string nextSceneName;
    public GameObject confirmPanel;
    public GameObject errorMessage;
    void Start()
    {
     
    
        // Hide everything by default
        nameInputPanel.SetActive(false);
        namePromptText.gameObject.SetActive(false);
        confirmPanel.gameObject.SetActive(false);
        introText.text = "";

        if (PlayerPrefs.HasKey("hasPlayedBefore"))
        {
            // Show returning intro with typewriter effect
            StartCoroutine(ShowReturningIntro(PlayerNameHandler.playerName));
        }
        else
        {
            // First time setup
            nameInputPanel.SetActive(true);
            namePromptText.gameObject.SetActive(true);

            // Auto-select the input field after it's active
            StartCoroutine(SelectInputFieldNextFrame());
        }
    }

    IEnumerator SelectInputFieldNextFrame()
    {
        // Wait one frame to ensure UI is fully active
        yield return null;
        EventSystem.current.SetSelectedGameObject(nameInputField.gameObject);
        nameInputField.ActivateInputField();
        confirmPanel.gameObject.SetActive(true);
    }

    public void ConfirmName()
    {
        string name = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("No name found");
            StartCoroutine(ErrorMessage(3f));
            return;
        }
        else
        {
            Debug.Log("Name confirmed: " + name);
            
        }

            PlayerNameHandler.SavePlayerName(name);

            // Hide input UI
            namePromptText.gameObject.SetActive(false);
            nameInputPanel.SetActive(false);

            // Start first intro typewriter effect
            StartCoroutine(ShowFirstIntro(name));
   
    }

    public IEnumerator ErrorMessage(float s)
    {
        errorMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(s);
        errorMessage.gameObject.SetActive(false);
       
    }

    IEnumerator TypeWriterEffect(string text, float speed)
    {
        introText.text = "";
        int i = 0;

        while (i < text.Length)
        {
            // If we encounter a rich text tag, e.g., <color=#FF0000>
            if (text[i] == '<')
            {
                int closingIndex = text.IndexOf('>', i);
                if (closingIndex != -1)
                {
                    // Add the whole tag instantly
                    string tag = text.Substring(i, closingIndex - i + 1);
                    introText.text += tag;
                    i = closingIndex + 1;
                    continue;
                }
            }

            // Add one visible character at a time
            introText.text += text[i];
            i++;

            yield return new WaitForSeconds(speed);
        }
    }

    IEnumerator ShowFirstIntro(string name)
    {
        introText.text = "";

        // Use DateTime display from DateTimeDisplay
        string dateTime = DateTime.Now.ToString("[hh:mm tt]");

        // Display the real-time DateTime in the intro
        yield return StartCoroutine(TypeWriterEffect(dateTime + " ACCESSING MEMORY.LOG...\n", 0.03f));
        yield return WaitForAnyKeyOrLeftClick();

        yield return StartCoroutine(TypeWriterEffect(dateTime + " WELCOME, " + name + "\n", 0.03f));
        yield return WaitForAnyKeyOrLeftClick();

        yield return StartCoroutine(TypeWriterEffect(dateTime + " OBSERVATION BEGINS.", 0.03f));
        yield return WaitForAnyKeyOrLeftClick();

        // Save that player has now played
        PlayerPrefs.SetInt("hasPlayedBefore", 1);
        PlayerPrefs.Save();
        LoadNextScene();
    }

    IEnumerator ShowReturningIntro(string name)
    {
        introText.text = "";

        string dateTime = DateTime.Now.ToString("[hh:mm tt]");

        yield return StartCoroutine(TypeWriterEffect(dateTime + " ACCESSING MEMORY.LOG...\n", 0.03f));
        yield return WaitForAnyKeyOrLeftClick();

        yield return StartCoroutine(TypeWriterEffect(dateTime + " WELCOME BACK, " + "<color=#FF0000>" + name + "</color>" + "\n", 0.03f));
        yield return WaitForAnyKeyOrLeftClick();

        yield return StartCoroutine(TypeWriterEffect(dateTime + " OBSERVATION RESUMED.", 0.03f));
        yield return WaitForAnyKeyOrLeftClick();

        LoadNextScene();
    }

    IEnumerator WaitForKeyPress(KeyCode key)
    {
        // Wait until the specified key is pressed
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }

    void LoadNextScene()
    {
        int uiVersion = PlayerPrefs.GetInt("UIVersion", 0);
        Debug.Log("Loading next scene based on UIVersion: " + uiVersion);


        if (PlayerPrefs.GetInt("UIVersion", 0) <= 1)
        {
            Debug.Log("playerpref UIVerion is 1 THEREFORE VER.1");
            SceneManager.LoadScene("Version1");
        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 2)
        {
            Debug.Log("playerpref UIVerion is 2 THEREFORE VER.2");
            SceneManager.LoadScene("Version2");

        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 3)
        {
            Debug.Log("playerpref UIVerion is 3 THEREFORE VER.3");
            SceneManager.LoadScene("Version3");

        }
        else if (PlayerPrefs.GetInt("UIVersion", 0) == 4)
        {
            Debug.Log("playerpref UIVerion is 3 THEREFORE VER.4");
            SceneManager.LoadScene("Version4");

        }
        else
        {
            Debug.LogWarning("Version not found");
        }

    }


    IEnumerator WaitForAnyKeyOrLeftClick()
    {
        yield return new WaitUntil(() =>
            Input.anyKeyDown || Input.GetMouseButtonDown(0)
        );
    }
}
