using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text textComponent;
    [TextArea] public string fullText;

    [Header("Typing Settings")]
    public float typeSpeed = 0.05f;
    public AudioSource typeSound;

    [Header("Shake Settings")]
    public float shakeIntensity = 0f;
    public float shakeSpeed = 0.02f;

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
                break; // still allow shake to start after skipping
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

        // Start infinite shake after typing completes or is skipped
        StartCoroutine(ShakeText(textComponent));
    }

    IEnumerator ShakeText(TMP_Text textComponent)
    {
        Vector3 originalPos = textComponent.transform.localPosition;

        while (true)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            textComponent.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            yield return new WaitForSeconds(shakeSpeed);
        }
    }
}

