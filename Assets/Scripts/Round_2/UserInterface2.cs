using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface2 : MonoBehaviour
{
    [Header("Typing Settings")]
    public float typeDelay = 0.03f;
    public AudioSource typeSound;

    [Header("Shake Settings")]
    public float shakeIntensity = 0.5f;

    [Header("UI References")]
    public TMP_Text fileTree;
    public TMP_Text filePreview;
    public TMP_Text pointerText;


    [Header("Text Content")]
    string treeText;
    string previewText;

    private bool skipTyping = false;

    // ✅ New flag
    public bool isTypingFinished { get; private set; } = false;

    private void Awake()
    {
        treeText = fileTree.text;
        previewText = filePreview.text;
    }

    void Start()
    {
        fileTree.text = "";
        filePreview.text = "";
        pointerText.gameObject.SetActive(false);
        StartCoroutine(TypeSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipTyping = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Intro");
        }
    }

    IEnumerator TypeSequence()
    {
        yield return StartCoroutine(TypeRichText(treeText, fileTree));
        skipTyping = false;

        pointerText.gameObject.SetActive(true);
        StartCoroutine(BlinkPrompt());

        yield return StartCoroutine(TypeRichText(previewText, filePreview));
        skipTyping = false;

        yield return new WaitForSeconds(1f);
        typeSound.Play();

        // ✅ Mark typing as complete
        isTypingFinished = true;
    }

    IEnumerator TypeRichText(string source, TMP_Text target)
    {
        StartCoroutine(ShakeText(target));
        int i = 0;

        while (i < source.Length)
        {
            if (skipTyping)
            {
                target.text = source;
                break;
            }

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

    IEnumerator ShakeText(TMP_Text textComponent)
    {
        Vector3 originalPos = textComponent.transform.localPosition;

        while (true)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            textComponent.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            yield return new WaitForSeconds(0.02f);
        }
    }


}

