using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    public TMP_Text textComponent;
    private string fullText;

    [Header("Typing Settings")]
    public float typeSpeed = 0.05f;
    public AudioSource typeSound;

    [Header("Shake Settings")]
    public bool enableShake = false;               // ✅ NEW: Toggle shaking in the Inspector
    public float shakeIntensity = 0f;
    public float shakeSpeed = 0.02f;

    private bool skipTyping = false;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        fullText = textComponent.text;
    }

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
                break;
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

        if (enableShake) // ✅ Only start shaking if enabled
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
