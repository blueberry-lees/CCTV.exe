using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Public Fields (Inspector)
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    [Header("Typing Settings")]
    public float typeDelay = 0.03f;
    public AudioSource typeSound;

    [Header("UI References")]
    public TMP_Text fileTree;
    public TMP_Text filePreview;
    public TMP_Text pointerText;
    public Image buttons;

    [Header("Text Content")]
    string treeText;
    string previewText;

    

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Private Fields
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    private bool skipTyping = false;

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Unity Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        treeText = fileTree.text;
        previewText = filePreview.text;

    }

    void Start()
    {
        // Initial Setup
        fileTree.text = "";
        filePreview.text = "";
        pointerText.gameObject.SetActive(false);
        buttons.gameObject.SetActive(false);

        // Start the typing sequence
        StartCoroutine(TypeSequence());
    }

    void Update()
    {
        // Handle skipping typing with spacebar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipTyping = true;
        }

        // Handle scene transition with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Intro");
        }
    }

    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    // Typing Coroutine Methods
    // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────

    IEnumerator TypeSequence()
    {
        // Type out treeText with typing effect
        yield return StartCoroutine(TypeRichText(treeText, fileTree));
        skipTyping = false; // Allow next typing sequence

        // Show pointer text and start blinking prompt
        pointerText.gameObject.SetActive(true);
        StartCoroutine(BlinkPrompt());

        // Type out previewText with typing effect
        yield return StartCoroutine(TypeRichText(previewText, filePreview));
        skipTyping = false; // Allow next typing sequence

        // Wait for 1 second, play type sound, and show buttons
        yield return new WaitForSeconds(1f);
        typeSound.Play();
        buttons.gameObject.SetActive(true);
    }

    IEnumerator TypeRichText(string source, TMP_Text target)
    {
        int i = 0;
        while (i < source.Length)
        {
            if (skipTyping)
            {
                target.text = source;
                yield break;
            }

            // Handle rich text tags
            if (source[i] == '<')
            {
                int tagEnd = source.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    while (i <= tagEnd)
                    {
                        target.text += source[i];
                        i++;
                    }
                    continue;
                }
            }

            // Add character to text and play typing sound if not whitespace
            target.text += source[i];
            if (!char.IsWhiteSpace(source[i]) && typeSound)
                typeSound.Play();

            i++;
            yield return new WaitForSeconds(typeDelay);
        }
    }

    IEnumerator BlinkPrompt()
    {
        while (true)
        {
            pointerText.alpha = pointerText.alpha == 1 ? 0 : 1;
            yield return new WaitForSeconds(1f);
        }
    }
}
