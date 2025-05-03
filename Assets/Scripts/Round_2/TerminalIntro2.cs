using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TerminalIntro2 : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Public Fields (Inspector)
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    [Header("Text Settings")]
    [TextArea(5, 20)]
    public string fullText;  // The full text to be displayed on the terminal
    public float typeDelay = 0.03f;  // Delay between each character while typing
    public AudioSource typeSound;  // Sound played during typing
    public TMP_Text terminalText;  // Text object displaying the terminal text
    public TMP_Text pressKeyPrompt;  // Prompt that tells the player to press a key to continue

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Private Fields
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    private bool finishedTyping = false;  // Flag to track if typing is finished
    private bool skipTyping = false;  // Flag to allow skipping typing animation

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Unity Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    void Start()
    {
        // Initialize flags and UI elements
        skipTyping = false;
        finishedTyping = false;

        terminalText.text = "";
        pressKeyPrompt.gameObject.SetActive(false);
        StartCoroutine(TypeTextWithRichSupport());
    }

    void Update()
    {
        // Handle space key input: Skip typing or load the next scene
        if (!finishedTyping && Input.GetKeyDown(KeyCode.Space))
        {
            skipTyping = true;
        }
        else if (finishedTyping && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Interface1");  // Load the next scene
        }
    }

    IEnumerator TypeTextWithRichSupport()
    {
        // Type the text one character at a time with support for rich text tags
        int i = 0;
        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                terminalText.text = fullText;
                break;
            }

            // Handle rich text tags (e.g., <b>, <i>, <color>)
            if (fullText[i] == '<')
            {
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    while (i <= tagEnd)
                    {
                        terminalText.text += fullText[i];
                        i++;
                    }
                    continue;
                }
            }

            // Add the character to the terminal text
            terminalText.text += fullText[i];

            // Play typing sound for non-whitespace characters
            if (!char.IsWhiteSpace(fullText[i]) && typeSound)
                typeSound.Play();

            i++;
            yield return new WaitForSeconds(typeDelay);  // Wait before typing the next character
        }

        finishedTyping = true;
        pressKeyPrompt.gameObject.SetActive(true);  // Show the press key prompt
        StartCoroutine(BlinkPrompt());
    }

    IEnumerator BlinkPrompt()
    {
        // Make the press key prompt blink
        while (true)
        {
            pressKeyPrompt.alpha = pressKeyPrompt.alpha == 1 ? 0 : 1;
            yield return new WaitForSeconds(0.7f);  // Toggle alpha every 0.7 seconds
        }
    }

   
}
