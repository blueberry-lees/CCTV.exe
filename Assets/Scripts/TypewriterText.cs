using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text textComponent;
    [TextArea] public string fullText;

    public float typeSpeed = 0.05f;
    public AudioSource typeSound;

    private void OnEnable()
    {
        textComponent.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        int i = 0;
        while (i < fullText.Length)
        {
            // If we hit a rich text tag, skip to the end of the tag
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

