using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterText2 : MonoBehaviour
{
    public TMP_Text textComponent;
    [TextArea] public string fullText;

    public float typeSpeed = 0.05f;
    public AudioSource typeSound;

    private bool skipTyping = false;


    private void OnEnable()
    {
        skipTyping = false;
        textComponent.text = "";
        StartCoroutine(TypeText());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skipTyping = true;
        }
    }

    IEnumerator TypeText()
    {
        int i = 0;
        while (i < fullText.Length)
        {
            if (skipTyping)
            {
                textComponent.text = fullText;
                yield break;
            }

            if (fullText[i] == '<')
            {
                int tagEnd = fullText.IndexOf('>', i);
                if (tagEnd != -1)
                {
                    while (i <= tagEnd)
                    {
                        textComponent.text += fullText[i];
                        i++;
                    }
                    continue;
                }
            }

            textComponent.text += fullText[i];

            if (!char.IsWhiteSpace(fullText[i]) && typeSound != null)
                typeSound.Play();

            i++;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}

