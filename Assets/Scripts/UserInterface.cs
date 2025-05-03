using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    [TextArea(5, 20)]
    public string treeText;

    [TextArea(5, 20)]
    public string previewText;

    public float typeDelay = 0.03f;
    public AudioSource typeSound;

    public TMP_Text fileTree;
    public TMP_Text filePreview;
    public TMP_Text pointerText;
    public Image buttons;

    private bool skipTyping = false;

    void Start()
    {
        fileTree.text = "";
        filePreview.text = "";
        pointerText.gameObject.SetActive(false);
        buttons.gameObject.SetActive(false);

        StartCoroutine(TypeSequence());
    }

    IEnumerator TypeSequence()
    {
        yield return StartCoroutine(TypeRichText(treeText, fileTree));
        skipTyping = false;//this
        pointerText.gameObject.SetActive(true);
        StartCoroutine(BlinkPrompt());
        skipTyping = false; //this

        yield return StartCoroutine(TypeRichText(previewText, filePreview));
        skipTyping = false;//this

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
}