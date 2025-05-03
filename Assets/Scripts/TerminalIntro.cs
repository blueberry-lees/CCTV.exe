using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TerminalIntro : MonoBehaviour
{
    [TextArea(5, 20)]
    public string fullText;

    public float typeDelay = 0.03f;
    public AudioSource typeSound;
    public TMP_Text terminalText;
    public TMP_Text pressKeyPrompt;

    private bool finishedTyping = false;
    private bool skipTyping = false;

    void Start()
    {
        skipTyping = false;
        finishedTyping = false;

        terminalText.text = "";
        pressKeyPrompt.gameObject.SetActive(false);
        StartCoroutine(TypeTextWithRichSupport());
    }

    IEnumerator TypeTextWithRichSupport()
    {
        int i = 0;
        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                terminalText.text = fullText;
                break;
            }

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

            terminalText.text += fullText[i];

            if (!char.IsWhiteSpace(fullText[i]) && typeSound)
                typeSound.Play();

            i++;
            yield return new WaitForSeconds(typeDelay);
        }

        finishedTyping = true;
        pressKeyPrompt.gameObject.SetActive(true);
        StartCoroutine(BlinkPrompt());
    }

    IEnumerator BlinkPrompt()
    {
        while (true)
        {
            pressKeyPrompt.alpha = pressKeyPrompt.alpha == 1 ? 0 : 1;
            yield return new WaitForSeconds(0.7f);
        }
    }

    void Update()
    {
        if (!finishedTyping && Input.GetKeyDown(KeyCode.Space))
        {
            skipTyping = true;
        }
        else if (finishedTyping && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Interface1");
        }
    }
}
